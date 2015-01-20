using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TdGraphsParts.Renderers.Graph;

namespace WindowsFormsGenerator
{
    interface ISchema
    {
        Action OnRedraw { get; set; }

        List<ISignalSource<float>> Build();

        void Start();

        void Stop();
    }
}
