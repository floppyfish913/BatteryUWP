//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Data.Xml.Dom;
//using Windows.ApplicationModel.Background;
//using Windows.UI.Notifications;
//using System.Diagnostics;

//namespace BGTask
//{
//    public sealed class bgListener : IBackgroundTask
//    {

//        public void Run(IBackgroundTaskInstance taskInstance)
//        {
//            Debug.WriteLine("   BACKGROUND PROCESS IS STARTED!!!!!!!!!!!!!!!!!!!!!!!!!!!");

//        }
        
//    }

//}

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Diagnostics;
using Windows.ApplicationModel.Background;


//
// The namespace for the background tasks.
//
namespace Tasks
{
    //
    // A background task always implements the IBackgroundTask interface.
    //
    public sealed class SampleBackgroundTask : IBackgroundTask
    {
       
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
           
            Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
            
        }

        
       
        }
    }
