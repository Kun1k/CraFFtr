using CraFFtr.Models;
using CraFFtr.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CraFFtr
{    
    public class CustomGrid : Grid
    {
        private static readonly int imageSizeRequest = 30;

        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
                                                        propertyName: "Item",
                                                        returnType: typeof(List<Item>),
                                                        declaringType: typeof(CustomGrid),
                                                        defaultValue: null,                                                        
                                                        defaultBindingMode: BindingMode.TwoWay,
                                                        propertyChanged: OnItemPropertyChanged);
        public List<Item> Item
        {
            get { return (List<Item>)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }      

        public CustomGrid()
        {                                  
        }        
        

        private static void PopulateGrid(CustomGrid cg, List<Item> items)
        {            
            var rowIndex = 1;            

            foreach(var item in items)
            {
                CreateControls(cg, item, rowIndex, 0);                
                rowIndex++;

                //has Sub-Items
                if (item.ItemRecipe != null)
                {
                    foreach (var ingredient in item.ItemRecipe.Ingredients)
                    {
                        CreateControls(cg, ingredient, rowIndex, 1);
                        rowIndex++;

                        if(ingredient.ItemRecipe != null)
                        {
                            foreach (var lastIngredient in ingredient.ItemRecipe.Ingredients)
                            {
                                CreateControls(cg, lastIngredient, rowIndex, 2);
                                rowIndex++;
                            }
                        }
                    }
                }


            }
        }

        private static void CreateControls(CustomGrid cg, Item item, int rowIndex, int columnIndex)
        {
            var label = new Label
            {
                Text = item.Ammount.ToString() + "x",
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center
            };
            label.SetDynamicResource(StyleProperty, "ListItemTextStyle");

            var image = new Image
            {
                Source = item.Icon,
                HeightRequest = imageSizeRequest,
                WidthRequest = imageSizeRequest
            };

            var itemNameLabel = new Label
            {
                Text = item.Name,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(20, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.Center
            };
            itemNameLabel.SetDynamicResource(StyleProperty, "ListItemTextStyle");


            cg.Children.Add(label, columnIndex, rowIndex);
            cg.Children.Add(image, columnIndex, rowIndex);
            cg.Children.Add(itemNameLabel, columnIndex + 1, rowIndex);

            Grid.SetColumnSpan(image, 2);
            Grid.SetColumnSpan(itemNameLabel, 5);
        }

        static void OnItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cg = (CustomGrid)bindable;            

            if(newValue != null)
            {
                var items = (List<Item>)newValue;

                PopulateGrid(cg, items);
                
                
            }            

            //< Image Grid.Column = "0" Grid.Row = "0" Grid.ColumnSpan = "3" Source = "{Binding Item.Icon}" HeightRequest = "50" WidthRequest = "50" />
            //< Label Text = "{Binding Item.Name}" Grid.ColumnSpan = "4" Grid.Column = "1" Grid.Row = "0" Style = "{DynamicResource ListItemTextStyle}" FontAttributes = "Bold" FontSize = "Medium" HorizontalTextAlignment = "Center" VerticalTextAlignment = "Center" />
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);

        //    foreach (View child in Children)
        //    {
        //        if (child.Width == 0 || child.Width < -1)
        //            child.HeightRequest = 0;
        //        else
        //            child.HeightRequest = -1;
        //    }
        //}
    }
}
