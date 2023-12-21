using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;


namespace lab6WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetPrepods(this.lPrepods);
        }
        private void GetPrepods(System.Windows.Controls.ListBox list)
        {
            string URL = @"https://www.mivlgu.ru/fakultety/fitr/pin/prepodavatelskiy-sostav";
            WebClient wClient = new WebClient();
            wClient.Encoding = Encoding.UTF8;
            HtmlDocument doc = new HtmlDocument(); //Создание документа HTML
            doc.LoadHtml(wClient.DownloadString(URL)); //Загрузка Html из интернета
            HtmlNodeCollection elements = doc.DocumentNode.
            SelectNodes("//div[contains(@class, 'teacher_inf') and @data-t-id]"); //получение последних добавленных фильмов
            List<Prepod> Films = new List<Prepod>();
            foreach (var el in elements)
            {
                Prepod prepod = new Prepod();
                HtmlNode info = el.Elements("div").Where(e =>
               e.Attributes["class"].Value == "general_inf").ToList()[0]; //получение блока содержащего информацию о фильме
                string imgUrl = URL;
                imgUrl = info.Elements("div").
                Where(e => e.Attributes["class"].Value ==
               "teacher_photo").ToList()[0].
               Element("a").Element("img").Attributes["src"].Value; //получение ссылки на изображение
                prepod.ImageUrl = new Uri(imgUrl);

                HtmlNode shortInfo = info.Elements("div").
                Where(e => e.Attributes["class"].Value ==
               "pers_data").ToList()[0]; //блок с краткой информацией
                prepod.Name = shortInfo.Elements("field").Where(e =>
               e.Attributes["class"].Value == "name field-inf").ToList()[0].Element("value").InnerText;

                prepod.Rank = shortInfo.Elements("field").Where(e =>
               e.Attributes["class"].Value == "title field-inf").ToList()[0].Element("value").InnerText;
                prepod.Post = shortInfo.Elements("field").Where(e =>
               e.Attributes["class"].Value == "post field-inf").ToList()[0].Element("value").InnerText;
                Films.Add(prepod);
                prepod.Post = shortInfo.Elements("field").Where(e =>
               e.Attributes["class"].Value == "cond field-inf").ToList()[0].Element("value").InnerText;

            }
            list.ItemsSource = Films; //Вывод информации в ListBox
        }
        class Prepod
        {
            string _name;
            string _rank;
            string _post;
            string _conditions;
            string _view;

            Uri _imageUrl;
            public Prepod() { }
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public string Rank
            {
                get { return _rank; }
                set { _rank = value; }
            }
            public string Post
            {
                get { return _post; }
                set { _post = value; }
            }
            public string Condition
            {
                get { return _conditions; }
                set { _conditions = value; }
            }
            public string View
            {
                get { return _view; }
                set { _view = value; }
            }
            public Uri ImageUrl
            {
                get { return _imageUrl; }
                set { _imageUrl = value; }
            }
        }
    }

}