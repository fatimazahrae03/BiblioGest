   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BiblioGest.ViewsBiblioGest.Views  5   	   '    !BiblioGest.Views.AdherentFormView   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:BiblioGest.Views     5   	   $	��600��5	   	   $	��550��6     � 
ResizeMode$  NoResize=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title 5   	   �� *WindowTitle+ � 	Resources 5       �� 
TargetType   
FieldLabel �� Property �� Value(  "System.Windows.Controls.DatePicker��%�  &	        '�� w     '  �     '�� _    �� 5   
   " ��6   .�5      ��  � ��$ 0,0,0,5q�6'   �� 5       � ��$ SemiBold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��$
 10,8q�6(   �� 5       � ��$ 0,0,0,15q�6'   5      �� 5   
   "  6   .�5      ��  � ��$
 10,5q�6(   �� 5       � ��$ 0,0,0,15q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$
 10,5q�6(   �� 5       � ��$ 0,0,0,15q�6'   5       5!      .��5#      �� ���#f8f9fa �� Padding$ 20q�6"   ._�5$   
   �  � RowDefinitions 5%      �� 5&      $
��Auto �6    �� 5'      $��* �6    �� 5(      $
��Auto �6    .+�5,      �� $��22(�5-      $
��Bold"�5.      $��0,0,0,20q�5/      ��5,      �� *WindowTitle+5/      �� 52      $��1��6   $
��Auto=�6(   .��53      �� .+�55      �� $��Nom *��6    #��[  6-   �� 56      ��6   �� *Adherent.Nom+	 �� UpdateSourceTrigger$	 PropertyChanged=��� 59      $��	Prénom *��6    #��[  60   �� 5:      ��6   �� *Adherent.Prenom+$	 PropertyChanged=��� 5=      $��Adresse��6    #��[  6/   �� 5>      $��60��5?      
 �� TextWrapping$

 Wrap=�5@       �� AcceptsReturn$
 True��5A      ��5>      �� *Adherent.Adresse+$	 PropertyChanged=�5A      �� 5D      $��Email��6    #��[  6-   �� 5E      ��6   �� *Adherent.Email+$	 PropertyChanged=��� 5H      $��Téléphone��6    #��[  61   �� 5I      ��6   �� *Adherent.Telephone+$	 PropertyChanged=��� 5L      $��Date d'inscription *��6    #��[  6<     5M         SelectedDate 6!   �� *Adherent.DateInscription+$	 PropertyChanged=��� 5P      $��Date de fin d'adhésion��6    #��[  6>     5Q       6!   �� *Adherent.DateFinAdhesion+$	 PropertyChanged=��� 5T      $��Statut *��6    #��[  60   �� 5U       �� DisplayMemberPath$ Label��5W      ��5U      �� *
StatusList+ �� SelectedItem 5V      �� *SelectedStatus+$	 PropertyChanged=�5W      �� 5Z      
���  ��$
��Wrap=�5\       $��0,0,0,15q�5^       ��5Z       �� *ErrorMessage+y�5]       �� *
HasError+   BooleanToVisibilityConverter �� 	Converter# [ 5^       5_      5`      � 5c      $��2��6   $��0,20,0,0q�6     � ColumnDefinitions 5d      �� 5e      $��* �6'   �� 5f      $
��Auto �6'   �� 5g      $
��Auto �6'   .+�5j      �� $��1��6   $��Annuler��5k      $	��100��5l      $��0,0,10,0q�5m      $
��10,8q�5n      ���#e0e0e0$��0q�5p      ��5q      �� *CancelCommand+�� 5s      $��2��6   $��Enregistrer��5t      $	��100��5u      $
��10,8q�5v      ���#27ae60
�������$��0q�5y      ��5z      �� *SaveCommand+5{      � 5~      $��3��6   y�5      �� *	IsLoading+# [ .+�5�      �� ���	#800000006   �� 5�      $��Center=�6   $��Center=�68   .+�5�      �� $��Traitement en cours...��6    
�������$��16(�5�       $��0,0,0,10q�5�       � 5�       � IsIndeterminate$
 True��6"   $	��200��69   $��10��6E   5�      5�      5�      5�      5�      