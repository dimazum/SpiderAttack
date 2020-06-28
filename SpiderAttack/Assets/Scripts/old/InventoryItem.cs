using System.Xml;
using System.Xml.Serialization;

//класс описывающий элемент инвенторя
public class InventoryItem {

    [XmlAttribute("itemName")] //аттрибут из файла
    public string itemName;

    public int cost; //цена элемента инвентаря
    public string icon;//имя иконки элемента инветаря

}
