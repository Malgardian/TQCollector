﻿using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System;

namespace TQCollector
{
    partial class Filterer
    {
        private struct rList
        {
            public WrapPanel w;
            public int h;
        }

        public static int ItemsTotal = 0;
        public static int ItemsCount = 0;
        private static List<rList> resizeList = new List<rList>();

        public static void resizeLists()
        {
            foreach (rList w in resizeList)
            {
                w.w.Height = (w.h / ((((int)Application.Current.MainWindow.Width - 10) / 200) - 1)) + 100;
            }
        }

        private static string generateText(string title, int sub, int tot)
        {
            if (Files.Configuration.UseItemCount)
            {
                return title + ": " + sub + "/" + tot + " (" + ((double)sub / tot * 100).ToString("N2") + "%)";
            }
            else
            {
                return title;
            }
        }

        // Removes any {.*} formatting on the item name
        private static string generateContent(string content)
        {
            int start = content.IndexOf('{');
            if (start == -1) start = 0;
            int end = content.LastIndexOf('}');
            if (end == -1) end = 0;
            else end++;

            return content.Remove(start, (end - start));
        }

        public static string generateSummary()
        {
            return "Total: " + ItemsCount + "/" + ItemsTotal + " (" + ((double)ItemsCount / ItemsTotal * 100).ToString("N2") + "%)";
        }

        public static TabControl Display()
        {
            //Reset item counts
            ItemsTotal = 0;
            ItemsCount = 0;
            resizeList = new List<rList>();

            //Create tabs
            TabControl MasterControl = new TabControl();

            if (Files.Configuration.Filters.MonsterInfrequent.Amount != Amount.None)
            {
                MasterControl.Items.Add(DisplayMonsterInfrequent());
            }
            if (Files.Configuration.Filters.Uniques.Amount != Amount.None)
            {
                MasterControl.Items.Add(DisplayUniques());
            }
            if (Files.Configuration.Filters.Artifacts.Amount != Amount.None && (Files.Configuration.UseIT || Files.Configuration.UseAE))
            {
                MasterControl.Items.Add(DisplayArtifacts());
            }
            if (Files.Configuration.Filters.Charms.Amount != Amount.None)
            {
                MasterControl.Items.Add(DisplayCharms());
            }
            if (Files.Configuration.Filters.Parchments.Amount != Amount.None && (Files.Configuration.UseIT || Files.Configuration.UseAE))
            {
                MasterControl.Items.Add(DisplayParchments());
            }
            if (Files.Configuration.Filters.Formulae.Amount != Amount.None && (Files.Configuration.UseIT || Files.Configuration.UseAE))
            {
                MasterControl.Items.Add(DisplayFormulae());
            }
            if (Files.Configuration.Filters.Scrolls.Amount != Amount.None && (Files.Configuration.UseIT || Files.Configuration.UseAE))
            {
                MasterControl.Items.Add(DisplayScrolls());
            }
            if (Files.Configuration.Filters.Relics.Amount != Amount.None)
            {
                MasterControl.Items.Add(DisplayRelics());
            }
            if (Files.Configuration.Filters.Sets.Amount != Amount.None)
            {
                MasterControl.Items.Add(DisplaySets());
            }

            return MasterControl;
        }

        private static TabItem _createTab(string header, Panel content)
        {
            TabItem ti = new TabItem();
            ti.Header = header;
            ti.VerticalContentAlignment = VerticalAlignment.Top;
            ti.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            ScrollViewer sv1 = new ScrollViewer();
            sv1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv1.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            sv1.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
            sv1.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            sv1.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            ti.Content = sv1;
            sv1.Content = content;

            return ti;
        }

        private static string _createTooltip(Item item)
        {
            string tooltip = "...";

            if (item.count > 0)
            {
                tooltip = string.Format(Files.Language["mouseover01"], item.count) + "\n";
                //List locations where the player has this item
                tooltip += System.String.Join("\n", item.locations.ToArray());
                tooltip += FormatFind(item.find);
            }
            else
            {
                tooltip = Files.Language["mouseover03"] + FormatFind(item.find);
            }

            return tooltip;
        }

        private static Label _createLabel(Item item, Thickness margin, int width, Thickness padding)
        {
            Label ct = new Label();

            ct.ToolTip = _createTooltip(item);
            ct.Margin = margin;
            if (item.count > 0 && Files.Configuration.UseItemNameCount)
            {
                ct.Content = "[" + item.count + "] " + generateContent(item.name);
            }
            else
            {
                ct.Content = generateContent(item.name);
            }
            ct.FontWeight = (item.count > 0) ? FontWeights.Bold : FontWeights.Normal;
            ct.Width = width;
            ct.Padding = padding;

            return ct;
        }

        private static CheckBox _createCheckBox(Item item)
        {
            CheckBox ct = new CheckBox();
            ct.IsEnabled = true;
            ct.ToolTip = _createTooltip(item);
            ct.Margin = new Thickness(5, 0, 0, 0);
            ct.Content = generateContent(item.name);
            ct.IsChecked = (item.count > 0);
            ct.Width = 200;
            ct.Foreground = Brushes.Black;

            return ct;
        }

        private static TabItem CreateTab(string header, Set[] set)
        {
            return _createTab(generateText(header, Files.Count(set), Files.Total(set)), CreatePanel(set));
        }

        private static TabItem CreateCompletionTab(string header, Set[] set)
        {
            return _createTab(generateText(header, Files.Count(set), Files.Total(set)), CreateCompletionPanel(set));
        }

        private static TabItem CreateListTab(string header, Set[] set)
        {
            return _createTab(generateText(header, Files.Count(set), Files.Total(set)), CreateListPanel(set));
        }

        private static StackPanel CreatePanel(Set[] set)
        {
            StackPanel sp = new StackPanel();

            foreach (Set s in set)
            {
                StackPanel sp2 = new StackPanel();
                sp2.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                sp2.Orientation = Orientation.Vertical;
                WrapPanel w = new WrapPanel();
                w.Orientation = Orientation.Horizontal;
                w.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                double percentage = 0;

                percentage = (double)Files.Count(s) / s.Item.Length * 100;
                //If empty, don't add a header
                TextBlock t = new TextBlock();
                if (!s.name.Equals(""))
                {
                    t.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    t.FontSize = 18;
                    t.FontStyle = FontStyles.Italic;
                    t.Text = Files.Database.exists(s.name) ? Files.Database.getName(s.name) : Files.Language[s.name] + ": " + Files.Count(s) + "/" + s.Item.Length + " (" + percentage.ToString("N2") + "%)";
                    sp2.Children.Add(t);
                }

                sp2.Children.Add(w);
                w.HorizontalAlignment = HorizontalAlignment.Stretch;

                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (Files.Configuration.UseIT || Files.Configuration.UseAE || !s.Item[i].isIT)
                    {
                        if (Files.Configuration.UseCheckBox)
                        {
                            w.Children.Add(_createCheckBox(s.Item[i]));
                        }
                        else
                        {
                            if (Files.Configuration.UseItemOwned || (!Files.Configuration.UseItemOwned && s.Item[i].count == 0))
                            {
                                w.Children.Add(_createLabel(s.Item[i], new Thickness(5, 0, 0, 3), Int32.Parse(Files.Language["width01"]), new Thickness(0)));
                            }
                        }
                    }
                }

                if (w.Children.Count == 0)
                {
                    sp2.Children.Remove(w);
                    sp2.Children.Remove(t);
                }
                sp.Children.Add(sp2);
            }

            return sp;
        }

        private static WrapPanel CreateListPanel(Set[] set)
        {
            WrapPanel sp = new WrapPanel();
            sp.Orientation = Orientation.Vertical;
            rList r = new rList();
            r.w = sp;
            r.h = (Files.Total(set) * 15) + (set.Length * 15);
            resizeList.Add(r);

            foreach (Set s in set)
            {
                StackPanel sp2 = new StackPanel();
                sp2.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                sp2.Orientation = Orientation.Vertical;
                WrapPanel w = new WrapPanel();
                w.Orientation = Orientation.Vertical;
                w.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                double percentage = 0;

                percentage = (double)Files.Count(s) / s.Item.Length * 100;
                //If empty, don't add a header
                TextBlock t = new TextBlock();
                if (!s.name.Equals(""))
                {
                    t.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    t.FontSize = 15;
                    t.Margin = new Thickness(0, 10, 0, 0);
                    t.FontStyle = FontStyles.Italic;
                    t.Text = Files.Database.exists(s.name) ? Files.Database.getName(s.name) : Files.Language[s.name];
                    sp2.Children.Add(t);
                }

                sp2.Children.Add(w);
                w.HorizontalAlignment = HorizontalAlignment.Stretch;

                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (Files.Configuration.UseIT || Files.Configuration.UseAE || !s.Item[i].isIT)
                    {
                        if (Files.Configuration.UseCheckBox)
                        {
                            w.Children.Add(_createCheckBox(s.Item[i]));
                        }
                        else
                        {
                            if (Files.Configuration.UseItemOwned || (!Files.Configuration.UseItemOwned && s.Item[i].count == 0))
                            {
                                w.Children.Add(_createLabel(s.Item[i], new Thickness(5, 0, 0, 0), Int32.Parse(Files.Language["width02"]), new Thickness(0)));
                            }
                        }
                        
                    }
                }
                if (w.Children.Count == 0)
                {
                    sp2.Children.Remove(w);
                    sp2.Children.Remove(t);
                }
                sp.Children.Add(sp2);
            }

            return sp;
        }

        private static WrapPanel CreateCompletionPanel(Set[] set)
        {
            WrapPanel sp = new WrapPanel();
            sp.Orientation = Orientation.Vertical;
            rList r = new rList();
            r.w = sp;
            r.h = (Files.Total(set) * 15) + (set.Length * 15);
            resizeList.Add(r);

            foreach (Set s in set)
            {
                StackPanel sp2 = new StackPanel();
                sp2.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                sp2.Orientation = Orientation.Vertical;
                WrapPanel w = new WrapPanel();
                w.Orientation = Orientation.Vertical;
                w.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                double percentage = 0;

                percentage = (double)Files.Count(s) / s.Item.Length * 100;
                //If empty, don't add a header
                TextBlock t = new TextBlock();
                if (!s.name.Equals(""))
                {
                    t.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    t.FontSize = 15;
                    t.Margin = new Thickness(0, 10, 0, 0);
                    t.FontStyle = FontStyles.Italic;
                    t.Text = Files.Database.exists(s.name) ? Files.Database.getName(s.name) : Files.Language[s.name];
                    sp2.Children.Add(t);
                }

                sp2.Children.Add(w);
                w.HorizontalAlignment = HorizontalAlignment.Stretch;

                for (int i = 0; i < s.Item.Length; i++)
                {
                    if (Files.Configuration.UseCheckBox)
                    {
                        w.Children.Add(_createCheckBox(s.Item[i]));
                    }
                    else
                    {
                        if (Files.Configuration.UseItemOwned || (!Files.Configuration.UseItemOwned && s.Item[i].count == 0))
                        {
                            w.Children.Add(_createLabel(s.Item[i], new Thickness(5, 0, 0, 0), Int32.Parse(Files.Language["width02"]), new Thickness(0)));
                        }
                    }
                }
                if (w.Children.Count == 0)
                {
                    sp2.Children.Remove(w);
                    sp2.Children.Remove(t);
                }
                sp.Children.Add(sp2);
            }

            return sp;
        }

        private static string FormatFind(string f)
        {
            if (f.Length < 1) return null;

            string s1 = "\n" + Files.Language["mouseover02"] + "\n";
            string s = f;

            if (f.Contains("@"))
            {
                do
                {
                    int i = s.IndexOf('@');
                    int j = s.IndexOf('$');
                    string lookup = s.Substring(i + 1, j - i - 1);
                    s = s.Replace("@" + lookup + "$", Files.Language[lookup]);
                } while (s.Contains("@"));
            }

            return s1 + s;
        }

        private static TabItem DisplayMonsterInfrequent()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;
            Set[] minormal;
            Set[] miepic;
            Set[] milegendary;

            if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.Some)
            {
                TabControl MonsterInfrequentTabControl = new TabControl();

                minormal = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Normal, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                miepic = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                milegendary = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);

                if (Files.Configuration.Filters.MonsterInfrequent.Normal)
                {
                    sub += Files.Count(minormal);
                    tot += Files.Total(minormal);
                    MonsterInfrequentTabControl.Items.Add(CreateListTab(Files.Language["level04"], minormal));
                }
                if (Files.Configuration.Filters.MonsterInfrequent.Epic)
                {
                    sub += Files.Count(miepic);
                    tot += Files.Total(miepic);
                    MonsterInfrequentTabControl.Items.Add(CreateListTab(Files.Language["level05"], miepic));
                }
                if (Files.Configuration.Filters.MonsterInfrequent.Legendary)
                {
                    sub += Files.Count(milegendary);
                    tot += Files.Total(milegendary);
                    MonsterInfrequentTabControl.Items.Add(CreateListTab(Files.Language["level06"], milegendary));
                }
 
                tb.Header = generateText(Files.Language["category01"], sub, tot);
                tb.Content = MonsterInfrequentTabControl;
            }
            else if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.All)
            {
                TabControl MonsterInfrequentTabControl = new TabControl();

                minormal = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Normal, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                miepic = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                milegendary = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                
                sub = Files.Count(minormal) + Files.Count(miepic) + Files.Count(milegendary);
                tot = Files.Total(minormal) + Files.Total(miepic) + Files.Total(milegendary);
                MonsterInfrequentTabControl.Items.Add(CreateListTab(Files.Language["level04"], minormal));
                MonsterInfrequentTabControl.Items.Add(CreateListTab(Files.Language["level05"], miepic));
                MonsterInfrequentTabControl.Items.Add(CreateListTab(Files.Language["level06"], milegendary));
                
                tb.Header = generateText(Files.Language["category01"], sub, tot);
                tb.Content = MonsterInfrequentTabControl;
            }
            else if (Files.Configuration.Filters.MonsterInfrequent.Amount == Amount.Any) // no MonsterInfrequentTabControl? Intentional? Investigate at some point.
            {
                minormal = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Normal, !Files.Configuration.UseBG, !Files.Configuration.UseIT, !Files.Configuration.UseSP, !Files.Configuration.UseR, !Files.Configuration.UseAtl, !Files.Configuration.UseEE);
                miepic = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Epic, !Files.Configuration.UseBG, !Files.Configuration.UseIT, !Files.Configuration.UseSP, !Files.Configuration.UseR, !Files.Configuration.UseAtl, !Files.Configuration.UseEE);
                milegendary = Files.filterItems(Files.ItemDatabase.MonsterInfrequent.Legendary, !Files.Configuration.UseBG, !Files.Configuration.UseIT, !Files.Configuration.UseSP, !Files.Configuration.UseR, !Files.Configuration.UseAtl, !Files.Configuration.UseEE);

                Set[] combined = Files.Combine(minormal, milegendary);
                combined = Files.Combine(combined, miepic);
                sub = Files.Count(combined);
                tot = Files.Total(combined);
                tb = CreateListTab(Files.Language["category01"], combined);
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem _createUniqueTabItem(Set[] uniques, Set[] sets, string heading, ref int sub, ref int tot)
        {
            TabItem ti = null;
            Set[] temp = null;
            
            if (Files.Configuration.Filters.Sets.Count)
            {
                //Make Set[] according to what we need (Secret Passage (&Ragnarök/Atlantis/Eternal Embers) or not)
                temp = Files.Add(Files.filterItems(sets, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE),
                                 Files.filterItems(uniques, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE));
            }
            else
            {
                temp = Files.filterItems(uniques, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
            }

            sub += Files.Count(temp);
            tot += Files.Total(temp);
            ti = CreateTab(heading, temp);

            return ti;
        }

        private static TabItem DisplayUniques()
        {
            TabItem tb = new TabItem();
            TabControl UniqueTabControl = new TabControl();
            int sub = 0;
            int tot = 0;

            if (Files.Configuration.Filters.Uniques.Amount == Amount.Some)
            {
                if (Files.Configuration.Filters.Uniques.Epic)
                {
                    UniqueTabControl.Items.Add(_createUniqueTabItem(Files.ItemDatabase.Uniques.Epic, Files.ItemDatabase.Sets.Epic, Files.Language["level05"], ref sub, ref tot));
                }
                if (Files.Configuration.Filters.Uniques.Legendary)
                {
                    UniqueTabControl.Items.Add(_createUniqueTabItem(Files.ItemDatabase.Uniques.Legendary, Files.ItemDatabase.Sets.Legendary, Files.Language["level06"], ref sub, ref tot));
                }
                tb.Header = generateText(Files.Language["category06"], sub, tot);
                tb.Content = UniqueTabControl;
            }
            else if (Files.Configuration.Filters.Uniques.Amount == Amount.All)
            {
                UniqueTabControl.Items.Add(_createUniqueTabItem(Files.ItemDatabase.Uniques.Epic, Files.ItemDatabase.Sets.Epic, Files.Language["level05"], ref sub, ref tot));
                UniqueTabControl.Items.Add(_createUniqueTabItem(Files.ItemDatabase.Uniques.Legendary, Files.ItemDatabase.Sets.Legendary, Files.Language["level06"], ref sub, ref tot));

                tb.Header = generateText(Files.Language["category06"], sub, tot);
                tb.Content = UniqueTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplayArtifacts()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;

            if (Files.Configuration.Filters.Artifacts.Amount == Amount.Some)
            {
                TabControl ArtifactTabControl = new TabControl();

                if (Files.Configuration.Filters.Artifacts.Normal)
                {
                    Set[] artnormal = Files.filterItems(Files.ItemDatabase.Artifacts.Normal, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                    sub += Files.Count(artnormal);
                    tot += Files.Total(artnormal);
                    ArtifactTabControl.Items.Add(CreateTab(Files.Language["level01"], artnormal));
                }
                if (Files.Configuration.Filters.Artifacts.Epic)
                {
                    Set[] artepic = Files.filterItems(Files.ItemDatabase.Artifacts.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                    sub += Files.Count(artepic);
                    tot += Files.Total(artepic);
                    ArtifactTabControl.Items.Add(CreateTab(Files.Language["level02"], artepic));
                }
                if (Files.Configuration.Filters.Artifacts.Legendary)
                {
                    Set[] artlegendary = Files.filterItems(Files.ItemDatabase.Artifacts.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                    sub += Files.Count(artlegendary);
                    tot += Files.Total(artlegendary);
                    ArtifactTabControl.Items.Add(CreateTab(Files.Language["level03"], artlegendary));
                }

                tb.Header = generateText(Files.Language["category04"], sub, tot);
                tb.Content = ArtifactTabControl;
            }
            else if (Files.Configuration.Filters.Artifacts.Amount == Amount.All)
            {
                TabControl ArtifactTabControl = new TabControl();

                Set[] artnormal = Files.filterItems(Files.ItemDatabase.Artifacts.Normal, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                Set[] artepic = Files.filterItems(Files.ItemDatabase.Artifacts.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                Set[] artlegendary = Files.filterItems(Files.ItemDatabase.Artifacts.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                sub = Files.Count(artnormal) + Files.Count(artepic) + Files.Count(artlegendary);
                tot = Files.Total(artnormal) + Files.Total(artepic) + Files.Total(artlegendary);
                ArtifactTabControl.Items.Add(CreateTab(Files.Language["level01"], artnormal));
                ArtifactTabControl.Items.Add(CreateTab(Files.Language["level02"], artepic));
                ArtifactTabControl.Items.Add(CreateTab(Files.Language["level03"], artlegendary));

                tb.Header = generateText(Files.Language["category04"], sub, tot);
                tb.Content = ArtifactTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplayCharms()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;

            if (Files.Configuration.Filters.Charms.Amount == Amount.All)
            {
                TabControl CharmsTabControl = new TabControl();

                Set[] chnormal = Files.filterItems(Files.ItemDatabase.Charms.Normal, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                Set[] chepic = Files.filterItems(Files.ItemDatabase.Charms.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                Set[] chlegendary = Files.filterItems(Files.ItemDatabase.Charms.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);

                sub = Files.Count(chnormal) + Files.Count(chepic) + Files.Count(chlegendary);
                tot = Files.Total(chnormal) + Files.Total(chepic) + Files.Total(chlegendary);
                CharmsTabControl.Items.Add(CreateTab(Files.Language["level04"], chnormal));
                CharmsTabControl.Items.Add(CreateTab(Files.Language["level05"], chepic));
                CharmsTabControl.Items.Add(CreateTab(Files.Language["level06"], chlegendary));

                tb.Header = generateText(Files.Language["category03"], sub, tot);
                tb.Content = CharmsTabControl;
            }
            if (Files.Configuration.Filters.Charms.Amount == Amount.Some)
            {
                TabControl CharmsTabControl = new TabControl();

                if (Files.Configuration.Filters.Charms.Normal)
                {
                    Set[] chnormal = Files.filterItems(Files.ItemDatabase.Charms.Normal, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                    sub += Files.Count(chnormal);
                    tot += Files.Total(chnormal);
                    CharmsTabControl.Items.Add(CreateTab(Files.Language["level04"], chnormal));
                }
                if (Files.Configuration.Filters.Charms.Epic)
                {
                    Set[] chepic = Files.filterItems(Files.ItemDatabase.Charms.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                    sub += Files.Count(chepic);
                    tot += Files.Total(chepic);
                    CharmsTabControl.Items.Add(CreateTab(Files.Language["level05"], chepic));
                }
                if (Files.Configuration.Filters.Charms.Legendary)
                {
                    Set[] chlegendary = Files.filterItems(Files.ItemDatabase.Charms.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                    sub += Files.Count(chlegendary);
                    tot += Files.Total(chlegendary);
                    CharmsTabControl.Items.Add(CreateTab(Files.Language["level06"], chlegendary));
                }

                tb.Header = generateText(Files.Language["category03"], sub, tot);
                tb.Content = CharmsTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplayParchments()
        {
            Set[] parch = Files.ItemDatabase.Parchments.Normal;
            if (!Files.Configuration.UseR)
            {
                parch = Files.removeR(parch);
            }
            if (!Files.Configuration.UseAtl)
            {
                parch = Files.removeAtl(parch);
            }
            if (!Files.Configuration.UseEE)
            {
                parch = Files.removeEE(parch);
            }
            TabItem tb = CreateTab(Files.Language["category07"], parch);
            int sub = Files.Count(parch);
            int tot = Files.Total(parch);

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplayFormulae()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;

            if (Files.Configuration.Filters.Formulae.Amount == Amount.All)
            {
                TabControl FormulaeTabControl = new TabControl();
                if (!Files.Configuration.UseR)
                {
                    if (!Files.Configuration.UseEE)
                    {
                        Set[] fornormal = Files.removeR(Files.ItemDatabase.Formulae.Normal);
                        Set[] forepic = Files.removeR(Files.ItemDatabase.Formulae.Epic);
                        Set[] forlegendary = Files.removeEE(Files.removeR(Files.ItemDatabase.Formulae.Legendary));
                        sub = Files.Count(fornormal) + Files.Count(forepic) + Files.Count(forlegendary);
                        tot = Files.Total(fornormal) + Files.Total(forepic) + Files.Total(forlegendary);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level01"], fornormal));
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level02"], forepic));
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level03"], forlegendary));
                    }
                    else
                    {
                        Set[] fornormal = Files.removeR(Files.ItemDatabase.Formulae.Normal);
                        Set[] forepic = Files.removeR(Files.ItemDatabase.Formulae.Epic);
                        Set[] forlegendary = Files.removeR(Files.ItemDatabase.Formulae.Legendary);
                        sub = Files.Count(fornormal) + Files.Count(forepic) + Files.Count(forlegendary);
                        tot = Files.Total(fornormal) + Files.Total(forepic) + Files.Total(forlegendary);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level01"], fornormal));
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level02"], forepic));
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level03"], forlegendary));
                    }
                }
                else
                {
                    Set[] forlegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        forlegendary = Files.removeEE(Files.ItemDatabase.Formulae.Legendary);
                    }
                    else
                    {
                        forlegendary = Files.ItemDatabase.Formulae.Legendary;
                    }
                    sub = Files.Count(Files.ItemDatabase.Formulae.Normal) + Files.Count(Files.ItemDatabase.Formulae.Epic) + Files.Count(forlegendary);
                    tot = Files.Total(Files.ItemDatabase.Formulae.Normal) + Files.Total(Files.ItemDatabase.Formulae.Epic) + Files.Total(forlegendary);
                    FormulaeTabControl.Items.Add(CreateTab(Files.Language["level01"], Files.ItemDatabase.Formulae.Normal));
                    FormulaeTabControl.Items.Add(CreateTab(Files.Language["level02"], Files.ItemDatabase.Formulae.Epic));
                    FormulaeTabControl.Items.Add(CreateTab(Files.Language["level03"], forlegendary));
                }
                tb.Header = generateText(Files.Language["category08"], sub, tot);
                tb.Content = FormulaeTabControl;
            }
            else if (Files.Configuration.Filters.Formulae.Amount == Amount.Some)
            {
                TabControl FormulaeTabControl = new TabControl();
                if (!Files.Configuration.UseR)
                {
                    Set[] fornormal;
                    Set[] forepic;
                    Set[] forlegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        fornormal = Files.removeR(Files.ItemDatabase.Formulae.Normal);
                        forepic = Files.removeR(Files.ItemDatabase.Formulae.Epic);
                        forlegendary = Files.removeEE(Files.removeR(Files.ItemDatabase.Formulae.Legendary));
                    }
                    else
                    {
                        fornormal = Files.removeR(Files.ItemDatabase.Formulae.Normal);
                        forepic = Files.removeR(Files.ItemDatabase.Formulae.Epic);
                        forlegendary = Files.removeR(Files.ItemDatabase.Formulae.Legendary);
                    }
                    if (Files.Configuration.Filters.Formulae.Normal)
                    {
                        sub += Files.Count(fornormal);
                        tot += Files.Total(fornormal);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level01"], fornormal));
                    }
                    if (Files.Configuration.Filters.Formulae.Epic)
                    {
                        sub += Files.Count(forepic);
                        tot += Files.Total(forepic);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level02"], forepic));
                    }
                    if (Files.Configuration.Filters.Formulae.Legendary)
                    {
                        sub += Files.Count(forlegendary);
                        tot += Files.Total(forlegendary);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level03"], forlegendary));
                    }
                }
                else
                {
                    Set[] forlegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        forlegendary = Files.removeEE(Files.ItemDatabase.Formulae.Legendary);
                    }
                    else
                    {
                        forlegendary = Files.ItemDatabase.Formulae.Legendary;
                    }
                    if (Files.Configuration.Filters.Formulae.Normal)
                    {
                        sub += Files.Count(Files.ItemDatabase.Formulae.Normal);
                        tot += Files.Total(Files.ItemDatabase.Formulae.Normal);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level01"], Files.ItemDatabase.Formulae.Normal));
                    }
                    if (Files.Configuration.Filters.Formulae.Epic)
                    {
                        sub += Files.Count(Files.ItemDatabase.Formulae.Epic);
                        tot += Files.Total(Files.ItemDatabase.Formulae.Epic);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level02"], Files.ItemDatabase.Formulae.Epic));
                    }
                    if (Files.Configuration.Filters.Formulae.Legendary)
                    {
                        sub += Files.Count(forlegendary);
                        tot += Files.Total(forlegendary);
                        FormulaeTabControl.Items.Add(CreateTab(Files.Language["level03"], forlegendary));
                    }
                }
                tb.Header = generateText(Files.Language["category08"], sub, tot);
                tb.Content = FormulaeTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplayScrolls()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;

            if (Files.Configuration.Filters.Scrolls.Amount == Amount.All)
            {
                TabControl ScrollsTabControl = new TabControl();
                if (!Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    if (!Files.Configuration.UseEE)
                    {
                        Set[] scnormal = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Normal));
                        Set[] scepic = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Epic));
                        Set[] sclegendary = Files.removeEE(Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Legendary)));
                        sub = Files.Count(scnormal) + Files.Count(scepic) + Files.Count(sclegendary);
                        tot = Files.Total(scnormal) + Files.Total(scepic) + Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                    else
                    {
                        Set[] scnormal = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Normal));
                        Set[] scepic = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Epic));
                        Set[] sclegendary = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Legendary));
                        sub = Files.Count(scnormal) + Files.Count(scepic) + Files.Count(sclegendary);
                        tot = Files.Total(scnormal) + Files.Total(scepic) + Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                else if (!Files.Configuration.UseR && Files.Configuration.UseAtl)
                {
                    if (!Files.Configuration.UseEE)
                    {
                        Set[] scnormal = Files.removeR(Files.ItemDatabase.Scrolls.Normal);
                        Set[] scepic = Files.removeR(Files.ItemDatabase.Scrolls.Epic);
                        Set[] sclegendary = Files.removeEE(Files.removeR(Files.ItemDatabase.Scrolls.Legendary));
                        sub = Files.Count(scnormal) + Files.Count(scepic) + Files.Count(sclegendary);
                        tot = Files.Total(scnormal) + Files.Total(scepic) + Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                    else
                    {
                        Set[] scnormal = Files.removeR(Files.ItemDatabase.Scrolls.Normal);
                        Set[] scepic = Files.removeR(Files.ItemDatabase.Scrolls.Epic);
                        Set[] sclegendary = Files.removeR(Files.ItemDatabase.Scrolls.Legendary);
                        sub = Files.Count(scnormal) + Files.Count(scepic) + Files.Count(sclegendary);
                        tot = Files.Total(scnormal) + Files.Total(scepic) + Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                else if (Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    if (!Files.Configuration.UseEE)
                    {
                        Set[] scnormal = Files.removeAtl(Files.ItemDatabase.Scrolls.Normal);
                        Set[] scepic = Files.removeAtl(Files.ItemDatabase.Scrolls.Epic);
                        Set[] sclegendary = Files.removeEE(Files.removeAtl(Files.ItemDatabase.Scrolls.Legendary));
                        sub = Files.Count(scnormal) + Files.Count(scepic) + Files.Count(sclegendary);
                        tot = Files.Total(scnormal) + Files.Total(scepic) + Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                    else
                    {
                        Set[] scnormal = Files.removeAtl(Files.ItemDatabase.Scrolls.Normal);
                        Set[] scepic = Files.removeAtl(Files.ItemDatabase.Scrolls.Epic);
                        Set[] sclegendary = Files.removeAtl(Files.ItemDatabase.Scrolls.Legendary);
                        sub = Files.Count(scnormal) + Files.Count(scepic) + Files.Count(sclegendary);
                        tot = Files.Total(scnormal) + Files.Total(scepic) + Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                else
                {
                    Set[] sclegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        sclegendary = Files.removeEE(Files.ItemDatabase.Scrolls.Legendary);
                    }
                    else
                    {
                        sclegendary = Files.ItemDatabase.Scrolls.Legendary;
                    }
                    sub = Files.Count(Files.ItemDatabase.Scrolls.Normal) + Files.Count(Files.ItemDatabase.Scrolls.Epic) + Files.Count(sclegendary);
                    tot = Files.Total(Files.ItemDatabase.Scrolls.Normal) + Files.Total(Files.ItemDatabase.Scrolls.Epic) + Files.Total(sclegendary);
                    ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], Files.ItemDatabase.Scrolls.Normal));
                    ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], Files.ItemDatabase.Scrolls.Epic));
                    ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                }
                tb.Header = generateText(Files.Language["category10"], sub, tot);
                tb.Content = ScrollsTabControl;
            }
            else if (Files.Configuration.Filters.Scrolls.Amount == Amount.Some)
            {
                TabControl ScrollsTabControl = new TabControl();
                if (!Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    Set[] scnormal;
                    Set[] scepic;
                    Set[] sclegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        scnormal = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Normal));
                        scepic = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Epic));
                        sclegendary = Files.removeEE(Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Legendary)));
                    }
                    else
                    {
                        scnormal = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Normal));
                        scepic = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Epic));
                        sclegendary = Files.removeAtl(Files.removeR(Files.ItemDatabase.Scrolls.Legendary));
                    }
                    if (Files.Configuration.Filters.Scrolls.Normal)
                    {
                        sub += Files.Count(scnormal);
                        tot += Files.Total(scnormal);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                    }
                    if (Files.Configuration.Filters.Scrolls.Epic)
                    {
                        sub += Files.Count(scepic);
                        tot += Files.Total(scepic);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                    }
                    if (Files.Configuration.Filters.Scrolls.Legendary)
                    {
                        sub += Files.Count(sclegendary);
                        tot += Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                else if (!Files.Configuration.UseR && Files.Configuration.UseAtl)
                {
                    Set[] scnormal;
                    Set[] scepic;
                    Set[] sclegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        scnormal = Files.removeR(Files.ItemDatabase.Scrolls.Normal);
                        scepic = Files.removeR(Files.ItemDatabase.Scrolls.Epic);
                        sclegendary = Files.removeEE(Files.removeR(Files.ItemDatabase.Scrolls.Legendary));
                    }
                    else
                    {
                        scnormal =Files.removeR(Files.ItemDatabase.Scrolls.Normal);
                        scepic = Files.removeR(Files.ItemDatabase.Scrolls.Epic);
                        sclegendary = Files.removeR(Files.ItemDatabase.Scrolls.Legendary);
                    }
                    if (Files.Configuration.Filters.Scrolls.Normal)
                    {
                        sub += Files.Count(scnormal);
                        tot += Files.Total(scnormal);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                    }
                    if (Files.Configuration.Filters.Scrolls.Epic)
                    {
                        sub += Files.Count(scepic);
                        tot += Files.Total(scepic);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                    }
                    if (Files.Configuration.Filters.Scrolls.Legendary)
                    {
                        sub += Files.Count(sclegendary);
                        tot += Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                else if (Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    Set[] scnormal;
                    Set[] scepic;
                    Set[] sclegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        scnormal = Files.removeAtl(Files.ItemDatabase.Scrolls.Normal);
                        scepic = Files.removeAtl(Files.ItemDatabase.Scrolls.Epic);
                        sclegendary = Files.removeEE(Files.removeAtl(Files.ItemDatabase.Scrolls.Legendary));
                    }
                    else
                    {
                        scnormal = Files.removeAtl(Files.ItemDatabase.Scrolls.Normal);
                        scepic = Files.removeAtl(Files.ItemDatabase.Scrolls.Epic);
                        sclegendary = Files.removeAtl(Files.ItemDatabase.Scrolls.Legendary);
                    }
                    if (Files.Configuration.Filters.Scrolls.Normal)
                    {
                        sub += Files.Count(scnormal);
                        tot += Files.Total(scnormal);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], scnormal));
                    }
                    if (Files.Configuration.Filters.Scrolls.Epic)
                    {
                        sub += Files.Count(scepic);
                        tot += Files.Total(scepic);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], scepic));
                    }
                    if (Files.Configuration.Filters.Scrolls.Legendary)
                    {
                        sub += Files.Count(sclegendary);
                        tot += Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                else
                {
                    Set[] sclegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        sclegendary = Files.removeEE(Files.ItemDatabase.Scrolls.Legendary);
                    }
                    else
                    {
                        sclegendary = Files.ItemDatabase.Scrolls.Legendary;
                    }
                    if (Files.Configuration.Filters.Scrolls.Normal)
                    {
                        sub += Files.Count(Files.ItemDatabase.Scrolls.Normal);
                        tot += Files.Total(Files.ItemDatabase.Scrolls.Normal);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level01"], Files.ItemDatabase.Scrolls.Normal));
                    }
                    if (Files.Configuration.Filters.Scrolls.Epic)
                    {
                        sub += Files.Count(Files.ItemDatabase.Scrolls.Epic);
                        tot += Files.Total(Files.ItemDatabase.Scrolls.Epic);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level02"], Files.ItemDatabase.Scrolls.Epic));
                    }
                    if (Files.Configuration.Filters.Scrolls.Legendary)
                    {
                        sub += Files.Count(sclegendary);
                        tot += Files.Total(sclegendary);
                        ScrollsTabControl.Items.Add(CreateTab(Files.Language["level03"], sclegendary));
                    }
                }
                tb.Header = generateText(Files.Language["category10"], sub, tot);
                tb.Content = ScrollsTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplayRelics()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;

            if (Files.Configuration.Filters.Relics.Amount == Amount.All)
            {
                TabControl RelicsTabControl = new TabControl();
                if (!Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    Set[] relnormal = Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Normal));
                    Set[] relepic = Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Epic));
                    Set[] rellegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Legendary)));
                    }
                    else
                    {
                        rellegendary = Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Legendary));
                    }
                    sub = Files.Count(relnormal) + Files.Count(relepic) + Files.Count(rellegendary);
                    tot = Files.Total(relnormal) + Files.Total(relepic) + Files.Total(rellegendary);
                    if (Files.Configuration.UseIT || Files.Configuration.UseAE)
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], relnormal));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], relepic));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                    else
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], Files.removeHades(relnormal)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], Files.removeHades(relepic)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], Files.removeHades(rellegendary)));
                    }
                }
                else if (!Files.Configuration.UseR && Files.Configuration.UseAtl)
                {
                    Set[] relnormal = Files.removeR(Files.ItemDatabase.Relics.Normal);
                    Set[] relepic = Files.removeR(Files.ItemDatabase.Relics.Epic);
                    Set[] rellegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(Files.removeR(Files.ItemDatabase.Relics.Legendary));
                    }
                    else
                    {
                        rellegendary = Files.removeR(Files.ItemDatabase.Relics.Legendary);
                    }
                    sub = Files.Count(relnormal) + Files.Count(relepic) + Files.Count(rellegendary);
                    tot = Files.Total(relnormal) + Files.Total(relepic) + Files.Total(rellegendary);
                    if (Files.Configuration.UseIT || Files.Configuration.UseAE)
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], relnormal));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], relepic));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                    else
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], Files.removeHades(relnormal)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], Files.removeHades(relepic)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], Files.removeHades(rellegendary)));
                    }
                }
                else if (Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    Set[] relnormal = Files.removeAtl(Files.ItemDatabase.Relics.Normal);
                    Set[] relepic = Files.removeAtl(Files.ItemDatabase.Relics.Epic);
                    Set[] rellegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(Files.removeAtl(Files.ItemDatabase.Relics.Legendary));
                    }
                    else
                    {
                        rellegendary = Files.removeAtl(Files.ItemDatabase.Relics.Legendary);
                    }
                    sub = Files.Count(relnormal) + Files.Count(relepic) + Files.Count(rellegendary);
                    tot = Files.Total(relnormal) + Files.Total(relepic) + Files.Total(rellegendary);
                    if (Files.Configuration.UseIT || Files.Configuration.UseAE)
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], relnormal));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], relepic));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                    else
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], Files.removeHades(relnormal)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], Files.removeHades(relepic)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], Files.removeHades(rellegendary)));
                    }
                }
                else
                {
                    Set[] rellegendary;
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(Files.ItemDatabase.Relics.Legendary);
                    }
                    else
                    {
                        rellegendary = Files.ItemDatabase.Relics.Legendary;
                    }
                    sub = Files.Count(Files.ItemDatabase.Relics.Normal) + Files.Count(Files.ItemDatabase.Relics.Epic) + Files.Count(rellegendary);
                    tot = Files.Total(Files.ItemDatabase.Relics.Normal) + Files.Total(Files.ItemDatabase.Relics.Epic) + Files.Total(rellegendary);
                    if (Files.Configuration.UseIT || Files.Configuration.UseAE)
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], Files.ItemDatabase.Relics.Normal));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], Files.ItemDatabase.Relics.Epic));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                    else
                    {
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], Files.removeHades(Files.ItemDatabase.Relics.Normal)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], Files.removeHades(Files.ItemDatabase.Relics.Epic)));
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], Files.removeHades(rellegendary)));
                    }
                }
                tb.Header = generateText(Files.Language["category02"], sub, tot);
                tb.Content = RelicsTabControl;
            }
            else if (Files.Configuration.Filters.Relics.Amount == Amount.Some)
            {
                TabControl RelicsTabControl = new TabControl();
                if (!Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    Set[] relnormal = Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Normal));
                    Set[] relepic = Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Epic));
                    Set[] rellegendary = Files.removeAtl(Files.removeR(Files.ItemDatabase.Relics.Legendary));
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(rellegendary);
                    }
                    if (Files.Configuration.Filters.Relics.Normal)
                    {
                        sub += Files.Count(relnormal);
                        tot += Files.Total(relnormal);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], relnormal));
                    }
                    if (Files.Configuration.Filters.Relics.Epic)
                    {
                        sub += Files.Count(relepic);
                        tot += Files.Total(relepic);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], relepic));
                    }
                    if (Files.Configuration.Filters.Relics.Legendary)
                    {
                        sub += Files.Count(rellegendary);
                        tot += Files.Total(rellegendary);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                }
                else if (!Files.Configuration.UseR && Files.Configuration.UseAtl)
                {
                    Set[] relnormal = Files.removeR(Files.ItemDatabase.Relics.Normal);
                    Set[] relepic = Files.removeR(Files.ItemDatabase.Relics.Epic);
                    Set[] rellegendary = Files.removeR(Files.ItemDatabase.Relics.Legendary);
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(rellegendary);
                    }
                    if (Files.Configuration.Filters.Relics.Normal)
                    {
                        sub += Files.Count(relnormal);
                        tot += Files.Total(relnormal);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], relnormal));
                    }
                    if (Files.Configuration.Filters.Relics.Epic)
                    {
                        sub += Files.Count(relepic);
                        tot += Files.Total(relepic);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], relepic));
                    }
                    if (Files.Configuration.Filters.Relics.Legendary)
                    {
                        sub += Files.Count(rellegendary);
                        tot += Files.Total(rellegendary);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                }
                else if (Files.Configuration.UseR && !Files.Configuration.UseAtl)
                {
                    Set[] relnormal = Files.removeAtl(Files.ItemDatabase.Relics.Normal);
                    Set[] relepic = Files.removeAtl(Files.ItemDatabase.Relics.Epic);
                    Set[] rellegendary = Files.removeAtl(Files.ItemDatabase.Relics.Legendary);
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(rellegendary);
                    }
                    if (Files.Configuration.Filters.Relics.Normal)
                    {
                        sub += Files.Count(relnormal);
                        tot += Files.Total(relnormal);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], relnormal));
                    }
                    if (Files.Configuration.Filters.Relics.Epic)
                    {
                        sub += Files.Count(relepic);
                        tot += Files.Total(relepic);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], relepic));
                    }
                    if (Files.Configuration.Filters.Relics.Legendary)
                    {
                        sub += Files.Count(rellegendary);
                        tot += Files.Total(rellegendary);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                }
                else
                {
                    Set[] rellegendary = Files.ItemDatabase.Relics.Legendary;
                    if (!Files.Configuration.UseEE)
                    {
                        rellegendary = Files.removeEE(rellegendary);
                    }
                    if (Files.Configuration.Filters.Relics.Normal)
                    {
                        sub += Files.Count(Files.ItemDatabase.Relics.Normal);
                        tot += Files.Total(Files.ItemDatabase.Relics.Normal);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level07"], Files.ItemDatabase.Relics.Normal));
                    }
                    if (Files.Configuration.Filters.Relics.Epic)
                    {
                        sub += Files.Count(Files.ItemDatabase.Relics.Epic);
                        tot += Files.Total(Files.ItemDatabase.Relics.Epic);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level08"], Files.ItemDatabase.Relics.Epic));
                    }
                    if (Files.Configuration.Filters.Relics.Legendary)
                    {
                        sub += Files.Count(Files.ItemDatabase.Relics.Legendary);
                        tot += Files.Total(Files.ItemDatabase.Relics.Legendary);
                        RelicsTabControl.Items.Add(CreateTab(Files.Language["level09"], rellegendary));
                    }
                }
                tb.Header = generateText(Files.Language["category02"], sub, tot);
                tb.Content = RelicsTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }

        private static TabItem DisplaySets()
        {
            TabItem tb = new TabItem();
            int sub = 0;
            int tot = 0;
            TabControl SetsTabControl = new TabControl();

            if (Files.Configuration.Filters.Sets.Amount == Amount.All)
            {
                Set[] epicSets = Files.filterItems(Files.ItemDatabase.Sets.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);
                Set[] legendarySets = Files.filterItems(Files.ItemDatabase.Sets.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);

                sub = Files.Count(epicSets) + Files.Count(legendarySets);
                tot = Files.Total(epicSets) + Files.Total(legendarySets);
                SetsTabControl.Items.Add(CreateListTab(Files.Language["level05"], epicSets));
                SetsTabControl.Items.Add(CreateListTab(Files.Language["level06"], legendarySets));
                
                tb.Header = generateText(Files.Language["category05"], sub, tot);
                tb.Content = SetsTabControl;
            }
            else if (Files.Configuration.Filters.Sets.Amount == Amount.Some)
            {
                if (Files.Configuration.Filters.Sets.Epic)
                {
                    Set[] epicSets = Files.filterItems(Files.ItemDatabase.Sets.Epic, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);

                    sub += Files.Count(epicSets);
                    tot += Files.Total(epicSets);
                    SetsTabControl.Items.Add(CreateListTab(Files.Language["level05"], epicSets));
                }
                if (Files.Configuration.Filters.Sets.Legendary)
                {
                    Set[] legendarySets = Files.filterItems(Files.ItemDatabase.Sets.Legendary, Files.Configuration.UseBG, Files.Configuration.UseIT, Files.Configuration.UseSP, Files.Configuration.UseR, Files.Configuration.UseAtl, Files.Configuration.UseEE);

                    sub += Files.Count(legendarySets);
                    tot += Files.Total(legendarySets);
                    SetsTabControl.Items.Add(CreateListTab(Files.Language["level06"], legendarySets));
                }
                tb.Header = generateText(Files.Language["category05"], sub, tot);
                tb.Content = SetsTabControl;
            }

            ItemsCount += sub;
            ItemsTotal += tot;

            return tb;
        }
    }
}
