﻿using System;
namespace ModuleLibraryiOS.Search
{
    public class SearchType
    {
        string uniqueId;
        string text;
        string description;
        string imageSource;

        public object instance;

        public SearchType(object inst, string id, string text, string description, string imageSource) 
        {
            this.instance = inst;
            this.uniqueId = id;
            this.text = text;
            this.imageSource = imageSource;
            this.description = description;
        }

        public string GetUniqueId() 
        {
            return uniqueId;
        }

        public string GetText()
        {
            return text;
        }

		public string GetDescription()
		{
            return description;
		}

        string GetImageSource() 
        {
            return imageSource;
        }
    }
}
