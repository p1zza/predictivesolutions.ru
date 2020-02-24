using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace predictivesolutions.ru
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Departments = new List<string> { };
        public List<string> Gndrs = new List<string> { };
        Person person = new Person();
        public XmlElement xRoot;
        public MainWindow()
        {
            InitializeComponent();
            start();
            TranslateAndSend(Gndrs);
        }
        public void start()
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                string mainpath = System.IO.Directory.GetCurrentDirectory();
                xDoc.Load("report.xml");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            xRoot = xDoc.DocumentElement;
            foreach (XmlNode xmlNode in xRoot)
            {
                if (xmlNode.Attributes.Count > 0)
                {
                    person.Name = xmlNode.Attributes.GetNamedItem("Name").Value;
                    NameList.Items.Add(person.Name);
                    person.Department = xmlNode.Attributes.GetNamedItem("Department").Value;
                    DepartmentList.Items.Add(person.Department);
                    person.Gender = xmlNode.Attributes.GetNamedItem("Gender").Value;
                    Gndrs.Add(person.Gender);
                }

                if (!Departments.Contains(xmlNode.Attributes.GetNamedItem("Department").Value))
                { Departments.Add(xmlNode.Attributes.GetNamedItem("Department").Value); }

                Department.ItemsSource = Departments;
            }
        }
        public void TranslateAndSend(List<string> genders)
        {
            foreach (string s in genders)
            {
                if(s == "male")
                {
                    GenderList.Items.Add("мужской");
                }
                else
                {
                    GenderList.Items.Add("женский");
                }
            }
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            List<string> names = new List<string>();
            foreach (var item in NameList.Items)
            {
                names.Add(item.ToString());
            }

            names.Sort();
            FlushList(NameList);
            FlushList(DepartmentList);
            FlushList(GenderList);
            foreach (string s in names)
            {

                foreach (XmlNode xmlNode in xRoot)
                {
                    if (xmlNode.Attributes.GetNamedItem("Name").Value == s)
                    {
                        List<string> _Gndrs = new List<string>();
                        NameList.Items.Add(s);
                        DepartmentList.Items.Add(xmlNode.Attributes.GetNamedItem("Department").Value);
                        _Gndrs.Add(xmlNode.Attributes.GetNamedItem("Gender").Value);
                        TranslateAndSend(_Gndrs);
                    }
                }
            }
        }

        private void FlushList(ListBox list)
        {
            int count = list.Items.Count;
            for (int i = 0; i < count; i++)
                list.Items.RemoveAt(0);
        }

        private void Department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ChoosedDepartment = Department.SelectedValue.ToString();
            FlushList(NameList);
            FlushList(DepartmentList);
            FlushList(GenderList);
                foreach (XmlNode xmlNode in xRoot)
                {
                    if (xmlNode.Attributes.GetNamedItem("Department").Value == ChoosedDepartment)
                    {
                        List<string> _Gndrs = new List<string>();
                        NameList.Items.Add(xmlNode.Attributes.GetNamedItem("Name").Value);
                        DepartmentList.Items.Add(xmlNode.Attributes.GetNamedItem("Department").Value);
                        _Gndrs.Add(xmlNode.Attributes.GetNamedItem("Gender").Value);
                        TranslateAndSend(_Gndrs);
                    }
                }
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
    }

}

