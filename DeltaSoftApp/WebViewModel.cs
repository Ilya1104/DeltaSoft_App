using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeltaSoftApp
{
    [FirestoreData]
    public class WebViewModel
    {
        [FirestoreProperty]
        public string url { get; set; }
    }
}
