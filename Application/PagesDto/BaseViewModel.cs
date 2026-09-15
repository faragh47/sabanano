using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.HrManagment.Polygon;
using Microsoft.EntityFrameworkCore.Internal;

namespace CleanArchitecture.Infrastructure.Common
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            IsSuccess = true;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        private bool _isCreatable;
        private bool _IsEditable;
        private bool _isView;
        public string ActionMessage { get; set; }
        public bool IsEditable
        {
            get => _IsEditable;
            set
            {
                _IsEditable = value;
                if (_IsEditable is true)
                {
                    ActionMessage = "ویرایش";
                    IsCreatable = false;
                    IsView = false;
                }
            }
        }

        public bool IsCreatable
        {
            get => _isCreatable;
            set
            {
                _isCreatable = value;
                if (_isCreatable is true)
                {
                    ActionMessage = "ثبت";
                    IsEditable = false;
                    IsView = false;
                }
            }
        }

        public bool IsView
        {
            get => _isView;
            set
            {
                _isView = value;
                if (_isView is true)
                {
                    ActionMessage = "";
                    IsEditable = false;
                    IsCreatable = false;
                }
            }
        }
    }
}