   M S B A M L   `   `   `  ����  DentalProApp!    DentalProApp.PacientiWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Gestionare Pacienți��5   	   $	��500��5   	   $	��800��6    � WindowStartupLocation$ CenterScreen=�5   	   ���#f9f9f9.��5	      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   ]� $��0��6   $��0 0 0 10q�6!   .+�5      �� $��📋 Listă pacienți��6   $��24(�61   $
��Bold"�6?   ���#333$��Center=�5      $
��Left=�63   �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��Right=�6I   .+�5      �� -   5        $��cmbCriteriu��5      $	��150��6.   $��0,0,10,0q�6:    �� SelectedIndex$ 0��6L   $��25��6^   .@�5      �� $
��Nume��6#   �� 5      $	��CNP��6#   �� 5      $��Telefon��6#   �� 5      $��Email��6#   5      �� 5      -   5         ��Name$ 
txtCautare��5      $	��191��6.   $��24��6_   5      5      &   System.Windows.Controls.DataGrid  5    
   -   5        $��
dgPacienti��5       $��1��6%      AutoGenerateColumns .     CanUserAddRows .     HeadersVisibility$ Column=�5#      	   RowBackground
	 �����
   AlternatingRowBackground
 �#f0f0f0   GridLinesVisibility$
 None=�5%      $��14(�5&      $��	0 10 0 10q�6!      
IsReadOnly .    SelectionMode$ Single=�5'   %      Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *id+6H     5+      $
 Nume��6%   $ * 6L    63   �� *nume+6L     5,      $ Prenume��6%   $ * 6R    66   �� *	prenume+6R     5-      $	 CNP��6%   $ * 6J    62   �� *cnp+6J     5.      $ Data nașterii��6%   $ * 6n    6<   �� *data_nasterii+ �� StringFormat$ d��6n     5/      $ Telefon��6%   $ * 6R    66   �� *	telefon+6R     50      $ Email��6%   $ * 6N    64   �� *email+6N     51      $ Adresă��6%   $ * 6P    65   �� *adresa+6P   53      �� 55   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   $��0 10 0 0q�6W    � 	Resources 56       �� 
TargetType �� Property �� Value��%W   '��       �� 57      " ��6   .�58      ��  � ��$ 5,0,0,0q�6/   59      5:      .+�5<      �� -   5        $��Adaugă��5<      $	��100��6&   ���#4CAF50
�������6Z   �� 5=      -   5        $��	Editează��5=      $	��100��6(   ���#2196F3
�������6\   �� 5>      -   5        $��Șterge��5>      $	��100��6&   ���#F44336
�������6Z   5?      5@      5A      