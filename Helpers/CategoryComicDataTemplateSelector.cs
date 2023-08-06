﻿using PicaComic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Bika.Helpers
{
    public class CategoryComicDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DontLoadTemplate { get; set; }


        public DataTemplate LoadTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            var category = item as CategoryComic;
            if (category == null)
            {
                return LoadTemplate;
            }
             
            return !BikaConfig.LoadLockComic && category.IsLocked ?  DontLoadTemplate: LoadTemplate;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
