   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BiblioGest.ViewsBiblioGest.Views  5      1    +BiblioGest.Views.EmpruntAdherentHistoryView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:BiblioGest.Views     5      .��5      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   $��0,0,0,15q�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� .+�5      �� $��22(�5      $��SemiBold"�5      $��Center=�5      $��0,0,0,5q�5      ��5      �� *	PageTitle+5      �� 5        �� Orientation$ 
Horizontal=�6   $��0,5,0,0q�66   .+�5!      �� $��Adhérent depuis le : ��6    $��SemiBold"�6=   �� 5"      $��5,0,0,0q�6k   ��6    �� *Adherent.DateInscription+ �� StringFormat$ {0:dd/MM/yyyy}��6k   �� 5$      $�� | Statut : ��6    $��SemiBold"�64   $��15,0,0,0q�6J   �� 5%      $��5,0,0,0q�6A   ��6    �� *Adherent.Statut+6A   5&      5'      �� 5)      $��1��6   $��Retour��5*      $
��10,5q�5+      ���#7f8c8d
�������$��0q�5.      ��5/      �� *RetourCommand+50      �� 53   
   $��0��6   $��	0,50,0,15q�54      
������� �� CornerRadius$ 5��56       �� Padding$ 15q�57      ._�58      �  59      �� 5:      $
��Auto �6'   �� 5;      $��* �6'   �� 5<      $
��Auto �6'   .+�5@      �� $��0��6   $ 
Horizontal=�6-   .+�5A      �� ���#3498db$ 5��5B      $ 10q�5C      $	��120��5D      $��0,0,10,0q�5E      ._�5F      �� .+�5G      �� $��Total��6(   
�������$��Center=�5I   (   �� 5J      $��22(�5K   (   $
��Bold"�5L   (   
�������$��Center=�5N   (   ��5J   (   �� *TotalEmprunts+5N   (   5O      5P      �� 5R      ���#2ecc71$ 5��5S      $ 10q�5T      $	��120��5U      $��0,0,10,0q�5V      ._�5W      �� .+�5X      �� $��Rendus��6(   
�������$��Center=�5Z   (   �� 5[      $��22(�5\   (   $
��Bold"�5]   (   
�������$��Center=�5_   (   ��5[   (   �� *EmpruntsRendus+5_   (   5`      5a      �� 5c      ���#e74c3c$ 5��5d      $ 10q�5e      $	��120��5f      ._�5g      �� .+�5h      �� $��En cours��6(   
�������$��Center=�5j   (   �� 5k      $��22(�5l   (   $
��Bold"�5m   (   
�������$��Center=�5o   (   ��5k   (   �� *EmpruntsEnCours+5o   (   5p      5q      5r      �� 5u      $��2��6   $ 
Horizontal=�6-   .+�5v      �� $	��150��6   $��0,0,10,0q�5w       �� DisplayMemberPath$ Label��5z      ��5x      �� *StatutFilterList+ �� SelectedItem 5y      �� *SelectedStatutFilter+5z      �� 5|      $��
Actualiser��6   $
��10,5q�5}      ���#3498db
�������$��0q�5�      ��5�      �� *RefreshCommand+5�      5�      5�      �� 5�   
   $��1��6   
�������$ 5��5�      $ 0q�5�      $��0,0,0,15q�5�      ._�5�      � .+�5�      &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��5�      	   CanUserAddRows$	 False��5�      
   CanUserDeleteRows$
 False��5�         
IsReadOnly$
 True��5�         GridLinesVisibility$ 
Horizontal=�5�         HeadersVisibility$ Column=�5�      $��0q�5�         RowHeaderWidth$ 0Q�5�         AlternatingRowBackground �#f9f9f9��5�      �� *
Emprunts+   Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 6W   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *	EmpruntId+6W     5�      $ Titre��6-   $ * 6g    6<   �� *Exemplaire.Livre.Titre+6g     5�      $ Auteur��6-   $	 150 6i    6=   �� *Exemplaire.Livre.Auteur+6i     5�      $ Date d'emprunt��6-   $	 120 6�    6E   �� *DateEmprunt+$ {0:dd/MM/yyyy}��6�     5�      $ Date de retour prévue��6-   $	 150 6�    6L   �� *DateRetourPrevue+$ {0:dd/MM/yyyy}��6�     5�      $ Date de retour effective��6-   $	 150 6�    6O   �� *DateRetourEffective+$ {0:dd/MM/yyyy}��6�     5�      $ Statut��6-   $	 100 6X    6=   �� *Statut+   	CellStyle 5�      �� 5�   "    �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6(    �� Triggers 5�   &   �� 5�   *    �� Binding 66   �� *Statut+ �� Value En cours6Q   .a�5�   .   ��  �� Property � �� �� Value �#e74c3c6K   �� 5�   .    � ��$ SemiBold"�6K   5�   +   �� 5�   *    66   �� *Statut+	 Rendu6Q   .a�5�   .   ��  � �� �#2ecc716K   5�   +   �� 5�   *    66   �� *Statut+ 	En retard6Q   .a�5�   .   ��  � �� �#e74c3c6K   �� 5�   .    � ��$
 Bold"�6K   5�   +   5�   #   5�      5�      � 5�      y�6   �� *	IsLoading+    BooleanToVisibilityConverter �� 	Converter# [  .+�5�      �� ���	#800000006    �� 5�      $��Center=�6!   $��Center=�6<   .+�5�      �� $��Chargement en cours...��6$   
�������$��16(�6U   $��0,0,0,10q�6c   � 5�       � IsIndeterminate$
 True��6&   $	��200��6=   $��10��6I   5�      5�      �� 5�      $/��)Aucun emprunt trouvé pour cet adhérent.��6   $��Center=�5�      $��Center=�5�      $��16(�5�      ���#7f8c8dy�5�      �� *HasNoEmprunts+# [  5�      5�      �� 5�   
   $��2��6   
�������$ 5��5�      $ 10q�5�      ._�5�      �� $ 
Horizontal=�6   $��Center=�5�      .+�5�      �� $��Précédent��6   $	��5,0q�5�      $
��10,5q�5�      ���#bdc3c7
�������$��0q�5�      ��5�      �� *PreviousPageCommand+{�5�      �� *CanGoToPreviousPage+�� 5�      $��Center=�5�      $
��15,0q�5�      ��5�      �� *
PageInfo+5�      �� 5�      $��Suivant��6   $	��5,0q�5�      $
��10,5q�5�      ���#bdc3c7
�������$��0q�5�      ��5�      �� *NextPageCommand+{�5�      �� *CanGoToNextPage+5�      5�      5�      5�      