using System;
using System.Collections.Generic;
using System.Text;
using ViewsDemo.Models;

namespace ViewsDemo.ViewModels
{
    public class LeaderViewModel : BaseViewModel
    {
        Leader Leader;

        public string Id => Leader.Id;
        public string Name => Leader.Name;
        public string ImageUrl => Leader.ImageUrl;
        public string JobTitle => Leader.JobTitle;

        public LeaderViewModel(Leader leader)
        {
            Leader = leader;
        }
    }
}
