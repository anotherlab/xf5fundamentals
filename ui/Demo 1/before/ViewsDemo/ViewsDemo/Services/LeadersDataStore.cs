using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewsDemo.Models;

namespace ViewsDemo.Services
{
    public class LeadersDataStore
    {
        List<Leader> leaders;

        public LeadersDataStore()
        {
            leaders = GetLeaders();
        }

        public List<Leader> GetAll()
        {
            return leaders;
        }

        public async Task<Leader> GetItemAsync(string id)
        {
            return await Task.FromResult(leaders.FirstOrDefault(s => s.Id == id));
        }

        private List<Leader> GetLeaders()
        {
            List<Leader> leaders = new List<Leader>();

            leaders.Add(new Leader
            {
                Name = "John Smyth",
                JobTitle = "Director of Fun",
                ImageUrl = "thum000.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Jane Linsey",
                JobTitle = "HR Manager",
                ImageUrl = "thum001.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Rob Chaney",
                JobTitle = "Dynamic Assurance Associate",
                ImageUrl = "thum002.jpg"
            });

            leaders.Add(new Leader
            {
                Name = "Esther Reeves",
                JobTitle = "Future Team Assistant",
                ImageUrl = "thum003.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Victor Conner",
                JobTitle = "Director of Deep Thinking",
                ImageUrl = "thum004.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Liz Torres",
                JobTitle = "Senior Assurance Guru",
                ImageUrl = "thum005.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Maria Ross",
                JobTitle = "Legacy Program Administrator",
                ImageUrl = "thum006.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "David Guo",
                JobTitle = "Corporate Accounts Manager",
                ImageUrl = "thum007.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Sabrina Gomez-Gardner",
                JobTitle = "Customer Happiness Director",
                ImageUrl = "thum008.jpg"
            });
            leaders.Add(new Leader
            {
                Name = "Robert Marcos",
                JobTitle = "Chief Tactics Associate",
                ImageUrl = "thum009.jpg"
            });
            return leaders;
        }
    }
}
