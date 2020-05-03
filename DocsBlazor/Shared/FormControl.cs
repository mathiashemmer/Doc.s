using Blazorise;
using Blazorise.Snackbar;
using DocsBlazor.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsBlazor.Pages
{

    public class FormControllBase<T> : BaseComponent
    {
        protected FormOperation CurrentOperation { get; set; } = FormOperation.Nenhum;

        //Snacbar | Toast controller
        public Snackbar Toast;
        public string ToastMessage { get; set; } = "";
        public int ToastDismissTime { get; set; } = 3000;

        //Form blocking modal controller
        public bool Blocked = false;

        protected virtual Task GetList()
        {
            throw new NotImplementedException();
        }

        protected virtual bool OnCancel()
        {
            throw new NotImplementedException();
        }

        protected virtual bool OnEdit(T model)
        {
            throw new NotImplementedException();
        }

        protected virtual bool OnView(T model)
        {
            throw new NotImplementedException();
        }

        protected virtual bool OnNew()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<bool> OnSave()
        {
            throw new NotImplementedException();
        }

        protected virtual Task<bool> OnDelete(T model)
        {
            throw new NotImplementedException();
        }

        public void ShowToast(string message = "", int time = 3000)
        {
            if(Toast != null)
            {
                this.ToastMessage = message;
                this.ToastDismissTime = time;
                Toast.Show();
            }
        }
        public void Block()
        {
            this.Blocked = true;
        }
        public void Unblock()
        {
            this.Blocked = false;
        }
        public string GetBlocked()
        {
            return this.Blocked ? "pointer-events: none" : "";
        }

    }
}
