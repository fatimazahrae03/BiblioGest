   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BiblioGest.ViewsBiblioGest.Views  5   	   +    %BiblioGest.Views.EmpruntHistoryWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:BiblioGest.Views     5   	     � Title$  Historique des emprunts��5   	   $	��450��5	   	   $	��700��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 	Resources 5         DefaultButtonStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " ��6+   .�5      ��  � �� �#3498db6+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 10,5q�6(   �� 5       �@�Cursor$
 Hand��6'   5      5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   $��0,0,0,15q�6    � ColumnDefinitions 5       �� 5!      $��* �6#   �� 5"      $
��Auto �6#   .+�5%      �� $��0��6   .+�5&      �� $��Historique des emprunts��6   $��20(�5'      $
��Bold"�5(      $��0,0,0,5q�5)      �� 5*      $��14(�5+      ��5*      �� *
HeaderInfo+5+      5,      5-      &   System.Windows.Controls.DataGrid  50   
   $��1��6      AutoGenerateColumns$ False��52      	   
IsReadOnly$
	 True��53      
   HeadersVisibility$
 Column=�54         GridLinesVisibility$ 
Horizontal=�55      $��1q�56      ���#ecf0f1
������    RowBackground
 �����   AlternatingRowBackground �#f8f9fa   CanUserAddRows$ False��5;         CanUserDeleteRows$ False��5<         SelectionMode$ Single=�5=         SelectionUnit$ FullRow=�5>      ��51      �� *
Emprunts+   Columns 5?      0  *System.Windows.Controls.DataGridTextColumn  5@      ,  &System.Windows.Controls.DataGridColumn   Header$ Date d'emprunt��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6|   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *DateEmprunt+ �� StringFormat$ {0:dd/MM/yyyy}��6|     5A      $ Date retour prévue��6%   $
 Auto 6�    6A   �� *DateRetourPrevue+$ {0:dd/MM/yyyy}��6�     5B      $ Date retour effective��6%   $
 Auto 6D    5C      `� 5D      $	 {0}��6'   .1�5E      ��  �� Path$ DateRetourEffective�6&   $ {0:dd/MM/yyyy}��6A    �� TargetNullValue$ -��6a   5F      5H        5I      $ Statut��6%   $
 Auto 6P    65   �� *Statut+6P     5J      $ 	Adhérent��6%   $ * 6W    67   �� *AdherentNom+6W     5K      $ 	Remarques��6%   $ * 6V    68   �� *	Remarques+6V   5M      �� 5P   
   $��2��6   $��Fermer��5Q      $��Right=�5R      $��0,15,0,0q�5S      $
��20,7q�5T      #��[  5V      ��5U      �� *CloseCommand+5V      5W      5X      