   M S B A M L   `   `   `  ����  DentalProApp)    #DentalProApp.EditareTratamentWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Editare Tratament��5   	   $	��520��6#   $	��420��60    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Editează tratamentul��6   $��20(�60   $
��Bold"�6>   $��0 0 0 10q�6P   �� 5   
   $��1��6   .+�5      �� $��Pacient:��6   $
��Bold"�6(   �� 5      -   5        $��
cmbPacient��5      $��30��6)   $��0,0,0,10q�65   �� 5      $��Medic:��6   $
��Bold"�6&   �� 5      -   5        $��cmbMedic��5      $��30��6'   $��0,0,0,10q�63   �� 5      $��Serviciu medical:��6   $
��Bold"�61   �� 5      -   5        $��cmbServiciu��5      $��30��6*   $��0,0,0,10q�66   �� 5      $��Data tratamentului:��6   $
��Bold"�63   (  "System.Windows.Controls.DatePicker  5      -   5        $��dpData��5      $��30��6'   $��0,0,0,10q�63   �� 5      $��Descriere tratament:��6   $
��Bold"�64   �� 5      -   5        $��txtDescriere��5      $��60��6*   $��0,0,0,10q�66    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6]   �� 5       $��Observații:��6   $
��Bold"�6+   �� 5!      -   5        $��txtObservatii��5!      $��60��6+   $��0,0,0,10q�67    . $
 Wrap=�6^   5"      �� 5$   
   -   5        $��2��5$      $��	Salvează��6   $��35��61   $	��120��6=   $��Center=�6I   $��0 20 0 0q�5%      ���#2196F3
�������$
��Bold"�5&   9   5'      5(      