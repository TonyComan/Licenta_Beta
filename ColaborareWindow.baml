   M S B A M L   `   `   `  ����  DentalProApp#    DentalProApp.ColaborareWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Colaborare Tehnicieni��5   	   $	��520��6'   $
��1000��64    � WindowStartupLocation$ CenterScreen=�5   	   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $2��,📝 Contracte de colaborare medic-tehnician��6   $��22(�6F   $
��Bold"�6T   ���#333$��0��6x   $��0 0 0 15q�6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��dgContracte��5      $��1��6&      AutoGenerateColumns .     
IsReadOnly .    HeadersVisibility$ Column=�5         SelectionMode$ Single=�6.      AlternatingRowBackground �#f0f0f0   RowBackground
 �����$��14(�5   L   	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 40 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *id+6H     5      $
 Medic��6%   $ * 6S    64   �� *
nume_medic+6S     5      $
 	Tehnician��6%   $ * 6[    68   �� *nume_tehnician+6[     5      $
 Email Tehnician��6%   $ * 6b    6>   �� *email_tehnician+6b     5      $
 Specializare��6%   $ * 6f    6;   �� *specializare_tehnician+6f     5      $
 Telefon��6%   $ * 6\    66   �� *telefon_tehnician+6\     5       $
 Durată (luni)��6%   $	 100 6\    6<   �� *durata_luni+6\   5"      �� 5%   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0 10 0 0q�6W   .+�5&      �� -   5        $��Adaugă��5&      $	��100��6&   $��5,0,0,0q�62   ���#4CAF50
�������6k   �� 5'      -   5        $��	Editează��5'      $	��100��6(   $��5,0,0,0q�64   ���#2196F3
�������6m   �� 5(      -   5        $��Șterge��5(      $	��100��6&   $��5,0,0,0q�62   ���#F44336
�������6k   �� 5)      -   5        $��
Export PDF��5)      $	��120��6*   $��10,0,0,0q�66   ���#9C27B0
�������6p   5*      5+      5,      