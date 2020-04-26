using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
namespace TodoListApp
{
    class TodoList
    {
        static private XmlDocument doc = new XmlDocument();
        static private Guid guid;
        static public ObservableCollection<Todo> Todos { get; set; }
        static async public void LoadTodoList()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
                doc.Load(ApplicationData.Current.LocalFolder.Path + @"\data.xml");
               
            }
            catch (System.IO.FileNotFoundException)
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("data.xml", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Root>\n</Root>");
                
                doc.Load(ApplicationData.Current.LocalFolder.Path + @"\data.xml");
            }

            using (XmlNodeList nodelist = doc.SelectNodes("Root/Todo")) 
            {
                List<XmlElement> list = nodelist.Cast<XmlElement>().ToList();
                Todos = new ObservableCollection<Todo>(from element in list
                                                     select new Todo(
                                                         element.SelectSingleNode("UID").InnerText,
                                                         element.SelectSingleNode("Title").InnerText,
                                                         element.SelectSingleNode("Description").InnerText,
                                                         Convert.ToBoolean(element.SelectSingleNode("isDone").InnerText)
                                                         ));
            }
        }

        static public void AddTodo(string title, string description)
        {
            //Generate ID
            if (string.IsNullOrWhiteSpace(title))
                return;
            guid = Guid.NewGuid();
            Todos.Add(new Todo(guid.ToString(), title, description, false));
            
        }

        static public void DeleteTodo(string id)
        {
            Todos.Remove(new Todo(id));
        }

        static public void ModifyTodo(string id, string title, string description)
        {
            //XmlElement element = doc.DocumentElement;
            //XmlElement modifynode = (XmlElement)element.SelectSingleNode(string.Format("/todo/task[@UID=\"{0}\"]", id));
            //if (modifynode == null)
            //    Debug.WriteLine("Khong tim thay ...");
            //else
            //{
            //    modifynode.SetAttribute("Title", title);
            //    modifynode.SetAttribute("Description", description);
            //    Debug.Write(doc.OuterXml);
            //    SaveData();
            //}
            Todos[Todos.IndexOf(new Todo(id))].Title = title;
            Todos[Todos.IndexOf(new Todo(id))].Description = description;
        }
        //static public void ChangeState(string id, bool state)
        //{
        //    Todos[Todos.IndexOf(new Todo(id))].isDone = state;
        //}
        static public Todo GetTodo(string id)
        {
            return Todos[Todos.IndexOf(new Todo(id))];
        }
        static public void SaveData()
        {
            
            FileStream datastream = new FileStream(ApplicationData.Current.LocalFolder.Path + @"\data.xml", FileMode.Truncate);
            StreamWriter writer = new StreamWriter(datastream, Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Todo>), new XmlRootAttribute("Root"));            
            serializer.Serialize(writer, Todos);
            //doc.Save(datastream);
            //datastream.Close();
        }
    }
}
