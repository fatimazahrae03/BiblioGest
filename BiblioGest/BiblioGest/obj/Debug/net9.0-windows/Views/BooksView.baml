   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BiblioGest.ViewsBiblioGest.Views  5           BiblioGest.Views.BooksView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:BiblioGest.Views     5      .��5
      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   $��0,0,0,15q�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5      �� $��0��6    �� CornerRadius$ 4��5      $��1q�5      ���#bdc3c7
������� �� Padding$ 5q�5       ._�5!      �  5"      �� 5#      $��* �6+   �� 5$      $
��Auto �6+   .+�5'      �� $��0��6   $��0q�5)      $��Center=�5*      $	��5,0q�5+      ��5(      �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=�5+      �� 5-      $��1��6   $
��🔍��5.      $��0q�50      
������ ���#7f8c8d$��5q�53       � Cursor$
 Hand��54      ��5/      �� *SearchCommand+54      55      56      �� 59      $��1��6   $��Catégorie:��5:      $��Center=�5;      $��15,0,5,0q�5<      �� 5>      $��2��6   $	��150��5A      $��0,0,15,0q�5B      ��5?      �� *FilterOptions+ �� SelectedItem 5@      �� *SelectedFilter+5B      �� 5E      $��3��6   $��+ Ajouter un livre��5F         DefaultButtonStyle#��[  5H      $
��15,5q�5I      ��5G      �� *AddBookCommand+5I      5J      �� 5M   
   $��1��6   
�������$ 5��5O      $��1q�5P      ���#ecf0f1._�5R      &   System.Windows.Controls.DataGrid  -   5           AutoGenerateColumns$ False��5T         
IsReadOnly$
 True��5U      	   HeadersVisibility$	 Column=�5V      
   GridLinesVisibility$
 
Horizontal=�5W      $��0q�5X      
������    RowBackground
 �����   AlternatingRowBackground �#f8f9fa   CanUserAddRows$ False��5\         CanUserDeleteRows$ False��5]         SelectionMode$ Single=�5^         SelectionUnit$ FullRow=�5_      ��5R      �� *DisplayedBooks+ 5S      �� *SelectedBook+   Columns 5a      0  *System.Windows.Controls.DataGridTextColumn  5b      ,  &System.Windows.Controls.DataGridColumn   Header$ Titre��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6R   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *Titre+6R     5c      $ Auteur��6)   $ * 6T    69   �� *Auteur+6T     5d      $
 ISBN��6)   $
 Auto 6P    67   �� *ISBN+6P     5e      $ Année��6)   $
 Auto 6R    68   �� *Annee+6R     5f      $ Éditeur��6)   $ * 6V    6:   �� *	Editeur+6V     5g      $ 
Catégorie��6)   $
 Auto 6^    6<   �� *Categorie.Nom+6^     5h      $ Exemplaires��6)   $
 Auto 6d    6>   �� *NombreExemplaires+6d   4  .System.Windows.Controls.DataGridTemplateColumn  5i      $ Actions��6-   $
 Auto 6>      CellTemplate 5j      �� 5k      .R�5l   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5m   &   �� $��✏️��6-    � ToolTip$ Modifier��5p   -   $��2q�5q   -   $	��5,0q�5r   -   
������ $��0q�5t   -   ��5n   -   �� *DataContext.EditBookCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5o   -   �� 5t   -   �� 5u   &   $��🗑️��6-   $ 	Supprimer��5x   -   $��2q�5y   -   $	��5,0q�5z   -   
������ $��0q�5|   -   ���#e74c3c��5v   -   �� *DataContext.DeleteBookCommand+ ��  DataGrid��5w   -   �� 5}   -   �� 5~   &   $��👁️��6-   $ Voir les détails��5�   -   $��2q�5�   -   $	��5,0q�5�   -   
������ $��0q�5�   -   ���#2980b9��5   -   �� *$"DataContext.ViewBookDetailsCommand+ ��  DataGrid��5�   -   �� 5�   -   5�   #   5�      5�      5�      5�      � 5�   
   $��2��6   $��0,15,0,0q�6    5�      �� 5�      $��* �6#   �� 5�      $
��Auto �6#   �� 5�      $
��Auto �6#   �� 5�      $
��Auto �6#   .+�5�      �� $��1��6   $��Center=�5�      $��0,0,15,0q�5�      ��5�      �� *CurrentPageDisplay+5�      �� 5�      $��2��6   $��◀ Précédent��5�      #��[  5�      $	��100��5�      $��0,0,5,0q�5�      ��5�      �� *PreviousPageCommand+5�      �� 5�      $��3��6   $��Suivant ▶��5�      #��[  5�      $	��100��5�      $��5,0,0,0q�5�      ��5�      �� *NextPageCommand+5�      5�      5�      5�      