   M S B A M L   `   `   `  ����  DentalProApp)    #DentalProApp.AdaugaProgramareWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Adaugă Programare��5   	   $	��400��6#   $	��400��60    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Adaugă o programare��6   $��20(�6/   $
��Bold"�6=   $��0 0 0 10q�6O   �� 5   
   $��1��6   .+�5      �� $��Pacient:��6   $
��Bold"�6(   �� 5      -   5        $��
cmbPacient��5      $��30��6)   $��0,0,0,10q�65   �� 5      $��Medic:��6   $
��Bold"�6&   �� 5      -   5        $��cmbMedic��5      $��30��6'   $��0,0,0,10q�63   �� 5      $��Data programării:��6   $
��Bold"�61   (  "System.Windows.Controls.DatePicker  5      -   5        $��dpData��5      $��30��6'   $��0,0,0,10q�63   �� 5      $��Ora programării (hh:mm):��6   $
��Bold"�68   �� 5      -   5        $��txtOra��5      $��30��6$   $��0,0,0,10q�60   5      �� 5   
   -   5        $��2��5      $��	Salvează��6   $��35��61   $	��120��6=   $��Center=�6I   $��0 20 0 0q�5      ���#4CAF50
�������$
��Bold"�5    9   5!      5"      