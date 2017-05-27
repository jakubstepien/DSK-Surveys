using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Views
{
    public interface IMainWindow
    {
        void LoadSurvey(Guid id);
    }
}
