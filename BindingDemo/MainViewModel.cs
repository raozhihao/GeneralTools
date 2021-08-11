using GeneralTool.General.WPFHelper;
using System;
using System.Windows.Input;

namespace BindingDemo
{
    class MainViewModel : BaseNotifyModel
    {
        private int a = 20;
        public int A { get => this.a; set => this.RegisterProperty(ref this.a, value); }

        private int b = 2;
        public int B { get => this.b; set => this.RegisterProperty(ref this.b, value); }

        public ICommand ChangedCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    this.B = new Random().Next(5, 30);
                });
            }
        }

        private Person person;
        public Person Person { get => this.person; set => this.RegisterProperty(ref this.person, value); }

        public MainViewModel()
        {
            this.Person = new Person() { Age = 21 };
        }
    }

    public class Person : BaseNotifyModel
    {
        private int age;
        public int Age { get => this.age; set => this.RegisterProperty(ref this.age, value); }
    }
}
