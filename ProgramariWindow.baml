   M S B A M L   `   `   `  ����  DentalProApp#    DentalProApp.ProgramariWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Gestionare Programări��5   	   $	��500��6'   $	��900��64    � WindowStartupLocation$ CenterScreen=�5   	   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��	0,10,0,10q�6.   $��Right=�6A   .+�5      �� -   5         ��Name$ cmbFiltruProgramari��5      $	��150��64   $��0,0,10,0q�6@   .@�5      �� $��Nume pacient��6   �� 5      $��
Nume medic��6   �� 5      $��Data programării��6   �� 5      $��Status��6   5      �� 5      -   5         ��Name$ txtCautareProgramari��5      $	��200��64   6@   5      �� 5   
   $��📋 Listă programări��6   $��24(�6/   $
��Bold"�6=   ���#333$��
0,0,430,10q�6a   &   System.Windows.Controls.DataGrid  5   
   -   5        $��dgProgramari��5      $��1��6'      AutoGenerateColumns .     
IsReadOnly .    HeadersVisibility$ Column=�5      	   SelectionMode$	 Single=�6.   
   AlternatingRowBackground
 �#f0f0f0   RowBackground
 �����$��14(�5   L      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *id+6H     5      $ Pacient��6%   $ * 6W    66   �� *nume_pacient+6W     5       $ Medic��6%   $ * 6S    64   �� *
nume_medic+6S     5!      $
 Data��6%   $ * 6g    63   �� *data_programare+ �� StringFormat$ d��6g     5"      $	 Ora��6%   $ * 6U    62   �� *ora_programare+6U     5#      $ Status��6%   $ * 6P    65   �� *status+6P   5%      �� 5'   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   $��0 10 0 0q�6W   .+�5(      �� -   5        $��Adaugă��5(      $	��100��6&   $��5,0,0,0q�62   ���#4CAF50
�������6k   �� 5)      -   5        $��	Editează��5)      $	��100��6(   $��5,0,0,0q�64   ���#2196F3
�������6m   �� 5*      -   5        $��Șterge��5*      $	��100��6&   $��5,0,0,0q�62   ���#F44336
�������6k   5+      5,      5-      