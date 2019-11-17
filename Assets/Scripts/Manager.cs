using UnityEngine;

public class Manager : MonoBehaviour
{
    private ItemTree cart1State;
    // Start is called before the first frame update
    void Start()
    {
        cart1State = new ItemTree();
        cart1State.Append("root", "ApplePie");
        cart1State.Append("ApplePie", "Apples");
        cart1State.Append("ApplePie", "Sugar");
        cart1State.Append("ApplePie", "Crust");
        cart1State.Append("Crust", "Milk");
        cart1State.Append("Crust", "Flour");

        /*
         * Tests -
         * GotItem ApplePie - Game Done
         * GotItem Apples, Crust, Sugar - Game Done
         * GotItem Apples, Milk, Flour, Sugar - Game Done
         * GotItem Apples, Crust, Flour, Sugar - Game Done
         * GotItem Apples, Milk, Crust, Sugar - Game Done
         */
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
    }

    private void Test1()
    {
        print("Test1 Start");
        ItemTree testTree = new ItemTree();
        testTree.Append("root", "ApplePie");
        testTree.Append("ApplePie", "Apples");
        testTree.Append("ApplePie", "Sugar");
        testTree.Append("ApplePie", "Crust");
        testTree.Append("Crust", "Milk");
        testTree.Append("Crust", "Flour");
        print("Test1 Result");
        GotItem(testTree, "ApplePie");
    }
    private void Test2()
    {
        print("Test2 Start");
        ItemTree testTree = new ItemTree();
        testTree.Append("root", "ApplePie");
        testTree.Append("ApplePie", "Apples");
        testTree.Append("ApplePie", "Sugar");
        testTree.Append("ApplePie", "Crust");
        testTree.Append("Crust", "Milk");
        testTree.Append("Crust", "Flour");
        print("Test2 Result");
        GotItem(testTree, "Apples");
        GotItem(testTree, "Crust");
        GotItem(testTree, "Sugar");
    }
    private void Test3()
    {
        print("Test3 Start");
        ItemTree testTree = new ItemTree();
        testTree.Append("root", "ApplePie");
        testTree.Append("ApplePie", "Apples");
        testTree.Append("ApplePie", "Sugar");
        testTree.Append("ApplePie", "Crust");
        testTree.Append("Crust", "Milk");
        testTree.Append("Crust", "Flour");
        print("Test3 Result");
        GotItem(testTree, "Apples");
        GotItem(testTree, "Milk");
        GotItem(testTree, "Flour");
        GotItem(testTree, "Sugar");
    }
    private void Test4()
    {
        print("Test4 Start");
        ItemTree testTree = new ItemTree();
        testTree.Append("root", "ApplePie");
        testTree.Append("ApplePie", "Apples");
        testTree.Append("ApplePie", "Sugar");
        testTree.Append("ApplePie", "Crust");
        testTree.Append("Crust", "Milk");
        testTree.Append("Crust", "Flour");
        print("Test4 Result");
        GotItem(testTree, "Apples");
        GotItem(testTree, "Crust");
        GotItem(testTree, "Flour");
        GotItem(testTree, "Sugar");
    }
    private void Test5()
    {
        print("Test5 Start");
        ItemTree testTree = new ItemTree();
        testTree.Append("root", "ApplePie");
        testTree.Append("ApplePie", "Apples");
        testTree.Append("ApplePie", "Sugar");
        testTree.Append("ApplePie", "Crust");
        testTree.Append("Crust", "Milk");
        testTree.Append("Crust", "Flour");
        print("Test5 Result");
        GotItem(testTree, "Apples");
        GotItem(testTree, "Milk");
        GotItem(testTree, "Crust");
        GotItem(testTree, "Sugar");
    }

    public void GotItem(ItemTree tree, string itemName)
    {
        print("Got " + itemName);
        if (tree.HasItem(itemName))
        {
            tree.ItemDone(itemName);
            if (tree.IsTreeDone())
            {
                print("Game Done");
            }
        }
        else
        {
            print("Bad Stuff");
        }

    }

    public void GotItem(string itemName)
    {
        print("Got " + itemName);
        if (this.cart1State.HasItem(itemName))
        {
            this.cart1State.ItemDone(itemName);
            if (this.cart1State.IsTreeDone())
            {
                print("Game Done");
            }
        }
        else
        {
            print("Bad Stuff");
        }

    }
}
