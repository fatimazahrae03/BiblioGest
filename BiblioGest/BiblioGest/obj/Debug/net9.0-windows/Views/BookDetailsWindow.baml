   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BiblioGest.ViewsBiblioGest.Views  5   	   (    "BiblioGest.Views.BookDetailsWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:BiblioGest.Views     5   	     � Title$  Détails du livre��5   	   $	��550��5	   	   $	��800��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 	Resources 5         DefaultButtonStyle �� 
TargetType �� Property �� Value  ActionButtonStyle �� BasedOn  ModifyButtonStyle  DeleteButtonStyle  AddButtonStyle   BooleanToVisibilityConverter��%�  &	        &	 �     7   &	 �    7  &	 �    7  &	 '    7   &	 z    �� 5   
   " ��6+   .�5      ��  � �� �#3498db6+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 10,5q�6(   �� 5       �@�Cursor$
 Hand��6'   5      �� 5   
   " ��6*   8   .�5      ��  � ��$	 8,3q�6(   �� 5       � ��$	 2,0q�6'   �� 5       � ��$ 11(�6)   5      �� 5   
   " ��6*   8   .�5      ��  � �� �#f39c126+   5      �� 5    
   " ��6*   8   .�5!      ��  � �� �#e74c3c6+   5"      �� 5$   
   " ��6'   8   .�5%      ��  � �� �#2ecc716+   5&      �� 5)   
   6'   5*      .��5,      � $��20q�6    � RowDefinitions 5-   
   �� 5.      $
��Auto �6   �� 5/      $��* �6   �� 50      $
��Auto �6   .+�54   
   � $��0��6    � ColumnDefinitions 55      �� 56      $
��Auto �6#   �� 57      $��* �6#   .+�5;      �� $��0��6   $	��120��5<      $	��160��5=      ���#f5f6fa$��1q�5?      ���#ecf0f1$��0,0,20,0q�5A      ._�5B      �� $��
Couverture��6   $��Center=�5C      $��Center=�5D      ���#bdc3c75E      5F      � 5I      $��1��6    5J      �� 5K      $
��Auto �6$   �� 5L      $
��Auto �6$   �� 5M      $
��Auto �6$   �� 5N      $
��Auto �6$   �� 5O      $
��Auto �6$   �� 5P      $��* �6$    5R      �� 5S      $
��Auto �6'   �� 5T      $��* �6'   .+�5X      �� $��0��6   $��0��6)   $��2��69   $��22(�5Z      $
��Bold"�5[      $��0,0,0,10q�5\      ��5Y      �� *Livre.Titre+5\      �� 5_      $��1��6   $��0��6)   $��Auteur:��5`      $��SemiBold"�5a      $��0,0,10,5q�5b      �� 5c      $��1��6   $��1��6)   $��0,0,0,5q�5e      ��5d      �� *Livre.Auteur+5e      �� 5h      $��2��6   $��0��6)   $��ISBN:��5i      $��SemiBold"�5j      $��0,0,10,5q�5k      �� 5l      $��2��6   $��1��6)   $��0,0,0,5q�5n      ��5m      �� *
Livre.ISBN+5n      �� 5q      $��3��6   $��0��6)   $��Publication:��5r      $��SemiBold"�5s      $��0,0,10,5q�5t      �� 5u      $��3��6   $��1��6)   $��0,0,0,5q�5w      ��5v      �� *PublicationInfo+5w      �� 5z      $��4��6   $��0��6)   $��Catégorie:��5{      $��SemiBold"�5|      $��0,0,10,5q�5}      �� 5~      $��4��6   $��1��6)   $��0,0,0,5q�5�      ��5      �� *Livre.Categorie.Nom+5�      �� 5�      $��5��6   $��0��6)   $��	Résumé:��5�      $��SemiBold"�5�      $��0,0,10,5q�5�      $	��Top=�5�      �� 5�      $��5��6   $��1��6)   $
��Wrap=�5�      $��0,0,0,5q�5�      ��5�      �� *Livre.Resume+5�      5�      5�      � 5�   
   $��1��6   $��0,20,0,0q�6    5�      �� 5�      $
��Auto �6    �� 5�      $��* �6    .+�5�      � $��0��6    5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   .+�5�      �� $��0��6   $��Exemplaires��5�      $��18(�5�      $��SemiBold"�5�      $��0,0,0,10q�5�      �� 5�      $��1��6   $��0,0,0,10q�5�      ��5�      �� *ExemplairesSummary+5�      5�      &   System.Windows.Controls.DataGrid  5�      -   5        $��1��5�      	   AutoGenerateColumns$	 False��5�      
   
IsReadOnly$

 True��5�         HeadersVisibility$ Column=�5�         GridLinesVisibility$ 
Horizontal=�5�      $��1q�5�      ���#ecf0f1
������    RowBackground
 �����   AlternatingRowBackground �#f8f9fa   CanUserAddRows$ False��5�         CanUserDeleteRows$ False��5�         SelectionMode$ Single=�5�         SelectionUnit$ FullRow=�5�      ��5�      �� *Exemplaires+ �� SelectedItem 5�      �� *SelectedExemplaire+ �� Mode$ TwoWay=�   Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$ Code Inventaire��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6e   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *CodeInventaire+6e     5�      $ État��6)   $
 Auto 6P    67   �� *Etat+6P     5�      $ Localisation��6)   $ * 6`    6?   �� *Localisation+6`   4  .System.Windows.Controls.DataGridTemplateColumn  5�      $ Statut��6-   $
 Auto 6=      CellTemplate 5�      �� 5�      .R�5�   "   � .+�5�   &   ��  �� Orientation$ 
Horizontal=�61   y�6J   �� *EstDisponible+ �� 	Converter# [ .+�5�   *   A� $��10��62   $��10��6=   ���#2ecc71$��0,0,5,0q�6X   �� 5�   *   $��
Disponible��64   ���#2ecc716F   5�   '   �� 5�   &   $ 
Horizontal=�61   ��5�   *   �� 5�   .   " ��64   .�5�   2   ��  � y�$ 	Collapsed=�6O    �� Triggers 5�   2   �� 5�   6    �� Binding 6B   �� *EstDisponible+ �� Value	 False6d   .a�5�   :   ��  � y�$ Visible=�6W   5�   7   5�   /   .+�5�   *   A� $��10��62   $��10��6=   ���#e74c3c$��0,0,5,0q�6X   �� 5�   *   $��	Emprunté��64   ���#e74c3c6D   5�   '   5�   #   5�      5�        5�      $ Actions��6-   $
 Auto 6>    5�      �� 5�      .R�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $��Modifier��6-   #��[ 5�   -   ��5�   -   �� *#!DataContext.EditExemplaireCommand+ �� RelativeSource ��   �� AncestorType
  Window��5�   -   �� �� 5�   &   $��	Supprimer��6-   #��[ 5�   -   ��5�   -   �� *%#DataContext.DeleteExemplaireCommand+ �� 
  Window��5�   -   �� 5�   #   5�      5�      ! � ToolTip! 5�      _� 5�      .��5�      �� $N��HDouble-cliquez sur un exemplaire pour afficher son historique d'emprunts��6$   5�      5�      5�      �� 5�   
   $��2��6   $ 
Horizontal=�5�      $��Right=�5�      $��0,20,0,0q�5�      .+�5     �� $��Ajouter un exemplaire��6   #��[ 5     $��0,0,10,0q�5     $
��15,7q�5     ��5     �� *AddExemplaireCommand+5     �� 5     $��Fermer��6   $
��20,7q�5	     #��[  5     ��5
     �� *CloseCommand+5     5     5     5     