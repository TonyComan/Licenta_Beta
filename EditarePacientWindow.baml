   M S B A M L   `   `   `  ����  DentalProApp'    !DentalProApp.EditarePacientWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Editare Pacient��5   	   $	��550��6!   $	��400��6.    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $"��Editează datele pacientului��6   $��20(�67   $
��Bold"�6E   $��0 0 0 10q�6W   �� 5   
   $��1��6   .+�5      �� $��Nume:��6   $
��Bold"�6%   �� 5      -   5        $��txtNume��5      $��30��6%   $��0,0,0,10q�61   �� 5      $��Prenume:��6   $
��Bold"�6(   �� 5      -   5        $��
txtPrenume��5      $��30��6(   $��0,0,0,10q�64   �� 5      $
��CNP:��6   $
��Bold"�6$   �� 5      -   5        $��txtCNP��5      $��30��6$    �� 	MaxLength$ 13��60   $��0,0,0,10q�6?   �� 5      $��Data nașterii:��6   $
��Bold"�6.   (  "System.Windows.Controls.DatePicker  5      -   5        $��dpDataNasterii��5      $��30��6/   $��0,0,0,10q�6;   �� 5      $��Telefon:��6   $
��Bold"�6(   �� 5      -   5        $��
txtTelefon��5      $��30��6(   $ 10��64   $��0,0,0,10q�6C   �� 5       $��Email:��6   $
��Bold"�6&   �� 5!      -   5        $��txtEmail��5!      $��30��6&   $��0,0,0,10q�62   �� 5#      $��Adresă:��6   $
��Bold"�6'   �� 5$      -   5        $��	txtAdresa��5$      $��30��6'   $��0,0,0,10q�63   5%      �� 5'   
   -   5        $��2��5'      $��	Salvează��6   $��35��61   $	��120��6=   $��Center=�6I   $��0 20 0 0q�5(      ���#2196F3
�������$
��Bold"�5)   9   5*      5+      