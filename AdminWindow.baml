   M S B A M L   `   `   `  ����  DentalProApp    DentalProApp.AdminWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Panou Administrator��5   	   $	��500��5   	   $	��700��6    � WindowStartupLocation$ CenterScreen=�5   	   ���#f9f9f9.��5	      � $��20q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Lista Utilizatori��6   $��24(�6-   $
��Bold"�6;   ���#333$��0 0 0 10q�5      &   System.Windows.Controls.DataGrid  5   
   -   5        $��dgUtilizatori��5      $��1��6(      AutoGenerateColumns .     CanUserAddRows .     HeadersVisibility$ Column=�5         RowBackground
 �����   AlternatingRowBackground �#f0f0f0   GridLinesVisibility$
 None=�5      $��14(�5      $��	0 10 0 10q�6!   	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *id+6H     5      $
 Username��6%   $ * 6T    67   �� *
username+6T     5      $	
 Rol��6%   $ * 6J    62   �� *rol+6J   5       �� 5"   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0 10 0 0q�6W    � 	Resources 5#       �� 
TargetType �� Property �� Value��%W   '��       �� 5$      " ��6   .�5%      ��  � ��$ 5,0,0,0q�6/   5&      5'      .+�5)      �� -   5        $��Adaugă��5)      $	��100��6&   ���#4CAF50
�������6Z   �� 5*      -   5        $��	Editează��5*      $	��100��6(   ���#2196F3
�������6\   �� 5+      -   5        $��Șterge��5+      $	��100��6&   ���#F44336
�������6Z   5,      5.      5/      