   M S B A M L   `   `   `  ����  DentalProApp,    &DentalProApp.ConsimtamantPacientWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$!  Consimțământ Chirurgical��5   	   $	��520��6*   $
��1000��67    � WindowStartupLocation$ CenterScreen=�5   	   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $-��'🤝 Consimțământ chrurgical pacient��6   $��22(�6>   $
��Bold"�6L   ���#333$��0 0 0 15q�5   &   &   System.Windows.Controls.DataGrid  5   
   -   5        $��dgConsimtamant��5      $��1��6)      AutoGenerateColumns .     
IsReadOnly . $��14(�5   %      AlternatingRowBackground �#f0f0f0   RowBackground
 �����   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 61   �� *id+6H     5      $ Pacient��6%   $	 * 6W   
 66   �� *nume_pacient+6W     5      $	 CNP��6%   $	 * 6J   
 62   �� *cnp+6J     5      $ Telefon��6%   $	 * 6R   
 66   �� *	telefon+6R     5      $ Medic��6%   $	 * 6S   
 64   �� *
nume_medic+6S     5      $
 Data��6%   $	 * 6e   
 63   �� *data_document+ �� StringFormat$ d��6e     5      $ 	Descriere��6%   $	 2* 6V   
 68   �� *	descriere+6V   5      �� 5"   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5#      �� -   5        $��Adaugă��5#      $	��100��6&   $��5q�62   ���#4CAF50
�������6e   �� 5$      -   5        $��	Editează��5$      $	��100��6(   $��5q�64   ���#2196F3
�������6g   �� 5%      -   5        $��Șterge��5%      $	��100��6&   $��5q�62   ���#F44336
�������6e   �� 5&      -   5        $��
Export PDF��5&      $	��120��6*   $��5q�66   ���#9C27B0
�������6i   5'      5(      5)      