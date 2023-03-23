package com.odospace.remotepanel;

import java.io.DataInputStream;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;
import java.nio.ByteBuffer;

import android.support.v7.app.ActionBarActivity;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.OrientationEventListener;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;

public class MainActivity extends ActionBarActivity {
	// maximum number of images
	private int MAX_IMAGES = 10;                          
	// image array
	private ImageView[] imageViews = new ImageView[MAX_IMAGES]; 
	// last update time
	private long[] lastUpdate = new long[MAX_IMAGES];
	// max wait time for socket timeout
	private int MAX_WAIT_TIME = 30000;
	
	// current number of images
	private int currentViews = 1;
	// ViewPager adapter
	private SectionsPagerAdapter mSectionsPagerAdapter;
	// allows to swipe between images
	private ViewPager mViewPager;
	// image listener thread
	private ImageListener imageListener = new ImageListener(this);
	// page indicator
	private LinePageIndicator titleIndicator = null;
	
	private OrientationEventListener mOrientationListener = null;
	
	@Override
	protected void onDestroy()
	{	
		// stop thread on destroy
		imageListener.stopRun();
		super.onDestroy();
	}
	
	
	@Override
	protected void onPostCreate(Bundle savedInstanceState) 
	{
		super.onPostCreate(savedInstanceState);
		// start listener thread
	    imageListener.start();  
	}
	
	
	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);

		try
		{
			// hide action bar an earlier android versions
			getSupportActionBar().hide();
		}
		catch (Throwable th)
		{
			
		}
		
		// Create the adapter that will return a fragment for each of the images
		mSectionsPagerAdapter = new SectionsPagerAdapter(getSupportFragmentManager());

		// Set up the ViewPager with the sections adapter.
		mViewPager = (ViewPager) findViewById(R.id.pager);
		mViewPager.setAdapter(mSectionsPagerAdapter);
        mViewPager.setOffscreenPageLimit(100);	// dont recreate images
		
		//Bind the title indicator to the adapter
		titleIndicator = (LinePageIndicator)findViewById(R.id.titles);
		titleIndicator.setViewPager(mViewPager);		
		titleIndicator.bringToFront();
		
		for (int i=0; i < MAX_IMAGES; i++)
			lastUpdate[i] = -1;
	}

	// ViewPager adapter
	public class SectionsPagerAdapter extends FragmentPagerAdapter 
	{

		public SectionsPagerAdapter(FragmentManager fm) {
			super(fm);
		}

		@Override
		public Fragment getItem(int position) {
			return PlaceholderFragment.newInstance(position + 1);  // create new image page
		}

		@Override
		public int getCount() {
			return currentViews;  // number of current images
		}
	}

	public void clearImage(final int imageId)
	{
		runOnUiThread(new Runnable() {
  		     @Override
  		     public void run() {
      		  // display default image
  		      if (imageViews[imageId] != null)
  		      {
  		        imageViews[imageId].setScaleType(ImageView.ScaleType.CENTER);
  			    imageViews[imageId].setImageResource(R.drawable.aida64);
  		      }
  		     }
  		});
	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
	    if ( keyCode == KeyEvent.KEYCODE_MENU ) {
	        // do nothing
	        return true;
	    }
	    return super.onKeyDown(keyCode, event);
	}  	
	
	// fragment that contains the image view
	public static class PlaceholderFragment extends Fragment 
	{
		// image id
		private static final String ARG_SECTION_NUMBER = "section_number";
		// main activity
		private MainActivity mainActivity = null;
		
		/**
		 * Returns a new instance of this fragment for the given section number.
		 */
		public static PlaceholderFragment newInstance(int sectionNumber) 
		{
			PlaceholderFragment fragment = new PlaceholderFragment();
			Bundle args = new Bundle();
			args.putInt(ARG_SECTION_NUMBER, sectionNumber);
			fragment.setArguments(args);
			return fragment;
		}

		public PlaceholderFragment() {
		}

		@Override
		public void onAttach(Activity activity) {
		    super.onAttach(activity);
		    mainActivity = (MainActivity) activity;  // remember main activity
		}		
		
		// create image view instance
		@Override
		public View onCreateView(LayoutInflater inflater, ViewGroup container,Bundle savedInstanceState) {
			View rootView = inflater.inflate(R.layout.fragment_main, container,false);
			
			Bundle args = getArguments();
			int sectionNumber = args.getInt(ARG_SECTION_NUMBER);
			
			// set default image
			ImageView image = (ImageView)rootView.findViewById(R.id.imageView);
			image.setImageResource(R.drawable.aida64);

			// remember image view
			mainActivity.imageViews[sectionNumber-1]=image;
			return rootView;
		}
	}

	  // image listener thread
	  class ImageListener extends Thread {
		    // main activity
		    private MainActivity mainActivity;
		    // continuation flag
		    public boolean cont = true;
		    // listener socket
		    ServerSocket serverSocket = null;
		    
		    public ImageListener(MainActivity act)
		    {
		    	this.mainActivity = act;
		    }
		    
		    public void stopRun()
		    {
		    	cont = false;
		    	try {
					serverSocket.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
		    }
		    
		    public void run() {
		      
		      int port = 38000;  // use default port
		      
		      try
		      {
		        serverSocket = new ServerSocket(port);
		        serverSocket.setSoTimeout(5000);

		        while (cont)
		        {
		          try
		          {
		            byte [] buf = new byte[4];
		            
		            // wait for image
		            Socket client = serverSocket.accept();
		            DataInputStream is = new DataInputStream(client.getInputStream());

		            // read command
		            is.read(buf);
		            ByteBuffer bb = ByteBuffer.wrap(buf);
		            final int command = bb.getInt();

		            // read image id
		            is.read(buf);
		            bb = ByteBuffer.wrap(buf);
		            final int imageId = bb.getInt();
		            
		            // read image size
		            is.read(buf);
		            bb = ByteBuffer.wrap(buf);
		            final int imageSize = bb.getInt();

		            // read image
		            byte [] image = null;
		            if (command == 2)
		            {
			            image = new byte[imageSize];
			            is.readFully(image);
		            }
		            
		            is.close();
		            client.close();
		            
		            // show image
		            if ((command == 2) && (imageId < MAX_IMAGES)) // show image request
		            {
		            	lastUpdate[imageId] = System.currentTimeMillis();
		            	
			           	final Bitmap bImage = BitmapFactory.decodeByteArray(image, 0, image.length);
			            
			    		runOnUiThread(new Runnable() {
				   		     @Override
				   		     public void run() 
				   		     {
				   		    	if (currentViews < (imageId+1))  // need to increase number of images to display
				   		    	{
					    	      currentViews = imageId+1;
					    	      mSectionsPagerAdapter.notifyDataSetChanged();
					    	      titleIndicator.invalidate();
				   		    	}
					    	    
				   		    	// dont center/scale image
					    	    Matrix mtrx = new Matrix();
					    	    mainActivity.imageViews[imageId].setScaleType(ImageView.ScaleType.MATRIX);  
					            mainActivity.imageViews[imageId].setImageMatrix(mtrx);
		
					            // display image
				   		    	mainActivity.imageViews[imageId].setImageBitmap(bImage);
				   		     }
			    		});
		            }
		            else
		            if ((command == 3) || (command == 1))  // clear screen request
		            	mainActivity.clearImage(imageId);
		          }
		          catch (SocketTimeoutException ste)
		          {
		        	  // retry later
		          }
		          catch (final Exception ex)
		          {
		        	  /*
		   		    	new AlertDialog.Builder(mainActivity)
			   		     .setTitle("Error")
			   		     .setMessage(ex.toString())
			   		     .setIcon(android.R.drawable.ic_dialog_alert)
			   		      .show();			   		    	 
					 */
		   		    	ex.printStackTrace();
		          }
		          finally
		          {
			    		runOnUiThread(new Runnable() {
				   		     @Override
				   		     public void run() {
			   		    	 	 // display timeout image
				   		    	 for (int i=0; i < MAX_IMAGES; i++)
				   		    	 {
				   		    		 long last = lastUpdate[i];
				   		    		 long current = System.currentTimeMillis();
				   		    		 
				   		    		 if ((last != -1) && ((last+MAX_WAIT_TIME) < current))
				   		    		 {
				   		    			mainActivity.clearImage(i);
				   		    			lastUpdate[i] = -1;   // sign, that currently the default image is displayed
				   		    		 }
				   		    	 }
				   		     }
				   		});
		          }
		        }
		        
		        serverSocket.close();
		      }
		      catch (final Exception ex)
		      {
		    	  /*
		    		runOnUiThread(new Runnable() {
			   		     @Override
			   		     public void run() {
			   		    	new AlertDialog.Builder(mainActivity)
				   		     .setTitle("Error")
				   		     .setMessage(ex.toString())
				   		     .setIcon(android.R.drawable.ic_dialog_alert)
				   		      .show();			   		    	 
			   		     }
			   		});
			   */		
		        ex.printStackTrace();
		      }
		    }
		  }
	
}
