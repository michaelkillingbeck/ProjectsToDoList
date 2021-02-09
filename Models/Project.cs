using Microsoft.Azure.Cosmos.Table;
using System;

namespace ProjectsToDoList.Models
{
    public class Project : TableEntity
    {
        public Project()
        {
        }

        public String ProjectName { get; set; }
    }
}