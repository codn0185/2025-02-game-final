using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    public abstract class Item
    {
        public abstract string Tag { get; }
        public abstract string Text { get; }
        public abstract Material Material { get; set; }
    }

    public class ATK_Speed : Item
    {
        public const string tag = "ATK_Speed";
        public const string text = "Speed UP";
        public static Material material = Resources.Load<Material>("Material/Fade01");

        public override string Tag
        {
            get { return tag; }
        }
        public override string Text
        {
            get { return text; }
        }
        public override Material Material
        {
            get { return material; }
            set { material = value; }
        }
    }

    public class ATK_Count : Item
    {
        public const string tag = "ATK_Count";
        public const string text = "Count Up";
        public static Material material = Resources.Load<Material>("Material/Fade02");

        public override string Tag
        {
            get { return tag; }
        }
        public override string Text
        {
            get { return text; }
        }
        public override Material Material
        {
            get { return material; }
            set { material = value; }
        }
    }

    public class ATK_Penetration : Item
    {
        public const string tag = "ATK_Penetration";
        public const string text = "Penetration Up";
        public static Material material = Resources.Load<Material>("Material/Fade03");

        public override string Tag
        {
            get { return tag; }
        }
        public override string Text
        {
            get { return text; }
        }
        public override Material Material
        {
            get { return material; }
            set { material = value; }
        }
    }

    public class ATK_Damage : Item
    {
        public const string tag = "ATK_Damage";
        public const string text = "Damage Up";
        public static Material material = Resources.Load<Material>("Material/Fade04");

        public override string Tag
        {
            get { return tag; }
        }
        public override string Text
        {
            get { return text; }
        }
        public override Material Material
        {
            get { return material; }
            set { material = value; }
        }
    }

    List<KeyValuePair<Item, int>> items;

    public float speed;
    const int ITEM_DISPLAY_COUNT = 2;
    public GameObject[] Cubes;
    public TextMeshProUGUI[] texts;

    public Material[] materials;

    void Start()
    {
        InitializeItemList();
        ShuffleItemList();
    }

    void Update()
    {
        transform.position -= speed * Time.deltaTime * transform.forward;
    }

    void InitializeItemList()
    {
        items = new() {
            new (new ATK_Count(), 10),
            new (new ATK_Speed(), 10),
            new (new ATK_Penetration(), 10),
            new (new ATK_Damage(), 10),
        };
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Key.Material = materials[i];
        }

    }

    void ShuffleItemList()
    {
        int n = items.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (items[n], items[k]) = (items[k], items[n]);
        }

        for (int i = 0; i < ITEM_DISPLAY_COUNT; i++)
        {
            Item item = items[i].Key;
            Cubes[i].GetComponent<Renderer>().material = item.Material;
            Cubes[i].tag = item.Tag;
            texts[i].text = item.Text;
        }
    }
}
