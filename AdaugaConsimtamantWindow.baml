   M S B A M L   `   `   `  ����  DentalProApp+    %DentalProApp.AdaugaConsimtamantWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Adaugă Consimțământ��5   	   $	��550��6%   $	��450��62    � WindowStartupLocation$ CenterScreen=�5   	   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $/��)Formular Consimțământ Inserare Implant��6   $��18(�6B   $
��Bold"�6P   $��0 0 0 20q�5      $��0��6&   �� 5   
   $��1��6   .+�5      �� $��Pacient:��6   $
��Bold"�6(   �� 5      -   5        $��cmbPacienti��5      $��30��6*   $��0 0 0 10q�66   6H   �� 5      $
��CNP:��6   $
��Bold"�6$   �� 5      -   5        $��txtCNP��5      $��30��6$    �� 
IsReadOnly . $��0 0 0 10q�6B   �� 5      $��Telefon:��6   $
��Bold"�6(   �� 5      -   5        $��
txtTelefon��5      $��30��6(    . $��0 0 0 10q�6F   �� 5      $��Medic:��6   $
��Bold"�6&   �� 5      -   5        $��	cmbMedici��5      $��30��6(   $��0 0 0 10q�64   �� 5      $��Descriere intervenție:��6   $
��Bold"�66   �� 5       -   5        $��txtDescriere��5       $��80��6*    �� TextWrapping$
 Wrap=�66    �� AcceptsReturn . $��0 0 0 10q�6_   �� 5"      $��Data consimțământului:��6   $
��Bold"�66   (  "System.Windows.Controls.DatePicker  5#      -   5        $��dpData��5#      $��30��6'   5$      �� 5&   
   -   5        $��2��5&      $��	Salvează��6   $��35��61   $	��150��6=   $��Center=�6I   ���#4CAF50
�������5'   9   5(      5)      