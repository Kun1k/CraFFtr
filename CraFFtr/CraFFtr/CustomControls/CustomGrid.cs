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
            var columnIndex = 0;
            var rowIndex = 1;

            foreach(var item in items)
            {
                //<Label Grid.Column="0" Grid.Row="1" Text=""  Style="{DynamicResource ListItemTextStyle}" FontAttributes="Bold" VerticalTextAlignment="Center"/>

                var label = new Label
                {
                    Text = item.Ammount.ToString() + "x",
                    FontAttributes = FontAttributes.Bold,                    
                    VerticalTextAlignment=TextAlignment.Center
                };
                label.SetDynamicResource(StyleProperty, "ListItemTextStyle");

                //<Image Grid.Column="1" Grid.Row="1" Source="" HeightRequest="40" WidthRequest="40"/>
                var image = new Image
                {
                    Source = item.Icon,
                    HeightRequest = 40,
                    WidthRequest = 40
                };


                //<Label Grid.Column="3" Grid.Row="1" VerticalTextAlignment="Center" Text=""  Style="{DynamicResource ListItemTextStyle}" FontAttributes="Bold"/>
                var itemNameLabel = new Label
                {
                    Text = item.Name,
                    FontAttributes = FontAttributes.Bold,
                    VerticalTextAlignment = TextAlignment.Center
                };
                itemNameLabel.SetDynamicResource(StyleProperty, "ListItemTextStyle");

                cg.Children.Add(label, 0, rowIndex);
                cg.Children.Add(image, 1, rowIndex);
                cg.Children.Add(itemNameLabel,2, rowIndex);

                //first population done -> check 2nd Level (subitems)
                rowIndex++;
                
                //has Sub-Items
                if(item.ItemRecipe != null)
                {

                }

                
            }


            //The parameters map to column, colspan, row, rowspan
            //cg.Children.Add(CONTROL, 0, 3, 0, 1);
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
