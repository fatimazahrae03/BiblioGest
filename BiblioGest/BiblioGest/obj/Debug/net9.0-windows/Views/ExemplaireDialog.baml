   M S B A M L   `   `   `  ����E  ABiblioGest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BiblioGest.ViewsBiblioGest.Views  5   	   '    !BiblioGest.Views.ExemplaireDialog   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:BiblioGest.Views     5   	   $	��300��5	   	   $	��500��5
   	     � WindowStartupLocation$  CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title 5   	   �� *WindowTitle+ � 	Resources 5         DefaultButtonStyle �� 
TargetType �� Property �� Value  SaveButtonStyle �� BasedOn  CancelButtonStyle  ErrorTextStyle  FormLabelStyle  FormTextBoxStyle  FormComboBoxStyle��%l  &	        &	 �     7   &	 =    7   &	 �    &	 +    &	 �    &	 `    �� 5   
   " ��6+   .�5      ��  � �� �#3498db6+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 10,5q�6(   �� 5       �@�Cursor$
 Hand��6'   5      �� 5   
   " ��6(   8   .�5      ��  � �� �#2ecc716+   5      �� 5   
   " ��6*   8   .�5      ��  � �� �#7f8c8d6+   5      �� 5   
   " ��6'   .�5       ��  � �� �#e74c3c6+   �� 5!       � ��$ 11(�6)   �� 5"       � ��$ 0,2,0,5q�6'   5#      �� 5%   
   " ��6'   .�5&      ��  � ��$ SemiBold"�6+   �� 5'       � ��$ 0,0,0,5q�6'   5(      �� 5*   
   " ��6)   .�5+      ��  � ��$ 5q�6(   �� 5,       � ��$ 0,0,0,10q�6'   �� 5-       � ��$ 1q�60   �� 5.       � �� �#bdc3c76,   5/      �� 51   
   " ��6*   .�52      ��  � ��$ 5q�6(   �� 53       � ��$ 0,0,0,10q�6'   �� 54       � ��$ 1q�60   �� 55       � �� �#bdc3c76,   56      57      .��59      � $��20q�6    � RowDefinitions 5:   
   �� 5;      $
��Auto �6   �� 5<      $��* �6   �� 5=      $
��Auto �6   .+�5A   
   �� $��0��6   $��20(�5C      $
��Bold"�5D      $��0,0,0,15q�5E      ��5B      �� *	FormTitle+5E      � 5H   
   $��1��6   	 � ColumnDefinitions	 5I      �� 5J      $��* �6#   �� 5K      $��* �6#    5M      �� 5N      $
��Auto �6    �� 5O      $
��Auto �6    �� 5P      $
��Auto �6    .+�5T      �� $��0��6   $��0��6&   $��0,0,10,0q�66   .+�5U      �� $��Code Inventaire:��6   #��[ 64   �� 5V      #��[ 5W      ��5V      �� *CodeInventaire+
 �� UpdateSourceTrigger$
 PropertyChanged=�5W      �� 5X      #��[ 5Y      ��5X      �� *ErrorCodeInventaire+y�5Z      �� *HasErrorCodeInventaire+   BooleanToVisibilityConverter �� 	Converter# [ 5[      �� 5^      $��0��6   $��1��6&   $��10,0,0,0q�66   .+�5_      �� $��État:��6   #��[ 6)   �� 5`      #��[ 5b      ��5`      �� *EtatsDisponibles+ �� SelectedItem 5a      �� *Etat+5b      �� 5c      #��[ 5d      ��5c      �� *	ErrorEtat+y�5e      �� *HasErrorEtat+# [ 5f      �� 5i      $��1��6   $��0��6&   $��2��66   .+�5j      �� $��Localisation:��6   #��[ 61   �� 5k      #��[ 5l      ��5k      �� *Localisation+$
 PropertyChanged=�5l      �� 5m      #��[ 5n      ��5m      �� *ErrorLocalisation+y�5o      �� *HasErrorLocalisation+# [ 5p      5q      �� 5t   
   $��2��6    �� Orientation$ 
Horizontal=�5u      $��Right=�5v      $��0,10,0,0q�5w      .+�5y      �� $��Annuler��6   #��[ 5{      $
��15,7q�5|      $��0,0,10,0q�5}      ��5z      �� *CancelCommand+5}      �� 5      $��Enregistrer��6   #��[ 5�      $
��15,7q�5�      ��5�      �� *SaveCommand+{�5�      �� *	CanSave+5�      5�      5�      5�      