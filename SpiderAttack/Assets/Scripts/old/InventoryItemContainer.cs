using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

//класс описывающий коллекцию инвентаря
[XmlRoot("InventoryItemCollection")]
public class InventoryItemContainer {

    [XmlArray("InventoryItems")]
    [XmlArrayItem("InventoryItem")]

    public List<InventoryItem> shopItems = new List<InventoryItem>();

}
