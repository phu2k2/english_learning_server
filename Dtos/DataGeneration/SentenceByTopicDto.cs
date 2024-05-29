using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace english_learning_server.Dtos.DataGeneration
{
    public class SentenceByTopicDto
    {
        public IEnumerable<string> Sentences { get; set; } = new List<string>();
    }
}