   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:BiblioGest.ViewModelsBiblioGest.ViewModels  5   	   "    BiblioGest.Views.LoginWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :
viewmodels#clr-namespace:BiblioGest.ViewModels     5   	     � Title$  Connexion Bibliothécaire��5   	   $	��400��6*   $	��500��67    � WindowStartupLocation$ CenterScreen=�5	   	    � DataContext 5      *   $BiblioGest.ViewModels.LoginViewModel  5   
   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��Connexion BiblioGest��5      $��24(�5      $
��Bold"�5      $��Center=�5      $��0,0,0,30q�5      �� 5!   
   $��1��6   $��0,0,0,15q�6"   .+�5"      �� $��Identifiant:��6   $��0,0,0,5q�6,   �� 5#      -   5         ��Name$ UserComboBox��5#      $��5q�5%      $��14(�5&       �� 
IsEditable$
 True��5'       �� StaysOpenOnEdit$
 True��5(       �� Text 5$      �� *Identifiant+ �� UpdateSourceTrigger$ PropertyChanged=�5(      5)      �� 5+   
   $��2��6   $��0,0,0,15q�6"   .+�5,      �� $��Mot de passe:��6   $��0,0,0,5q�6-   G� 5-      -   5        	 G�Name$	 PasswordBox��5-      $��5q�5.      $��14(�5/      50      �� 52   
   $��3��6   $��Se souvenir de moi��53      $��0,0,0,15q�55      
 d� 	IsChecked
 54      �� *
SeSouvenir+55      �� 57   
   $��4��6   
���  ��$��0,0,0,15q�5:      $
��Wrap=�5;      ��58      �� *ErrorMessage+5;      �� 5=   
   $��5��6   $��Se connecter��5>      $
��10,5q�5A      $��14(�5B      $��Center=�5C      $	��120��5D      ��5?      �� *LoginCommand+��5@      ��  �� ElementName$ PasswordBox��5D      5E      5F      