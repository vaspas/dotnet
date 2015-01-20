
using System;
using System.Collections.Generic;
using System.Linq;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class SelectedObjects : IExtension<TapeModel>
    {
        private class Pair
        {
            public Pair(object obj, object sender)
            {
                Object = obj;
                Sender = sender;
            }

            public object Object;
            public object Sender;
        }

        private readonly List<Pair> _mouseOverObjects = new List<Pair>();

        public void AddMouseOver(object obj, object sender)
        {
            if (_mouseOverObjects.Any(p => p.Object == obj && p.Sender == sender))
                return;

            _mouseOverObjects.Add(new Pair(obj, sender));

            MouseOverChanged();
        }

        public void RemoveMouseOverFor(object sender)
        {
            _mouseOverObjects.RemoveAll(p => p.Sender == sender);

            MouseOverChanged();
        }


        public event Action MouseOverChanged = delegate { };

        public IEnumerable<T> GetAllMouseOver<T>() where T:class
        {
            return _mouseOverObjects.Select(p=>p.Object).OfType<T>();
        }

        public void Build(TapeModel tapeModel)
        {
            
        }
    }
}
