   M S B A M L   `   `   `  ����  DentalProApp&     DentalProApp.AdaugaFacturaWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Adaugă Factură��5   	   $	��550��6    $	��500��6-    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.   ���#f9f9f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $-��'Facturare pacient - tratamente multiple��6   $��20(�6C   $
��Bold"�6Q   $��0 0 0 15q�6c   �� 5   
   $��1��6   .+�5      �� $��Selectează pacientul:��6   $
��Bold"�65   �� 5      -   5        $��
cmbPacient��5      $��30��6)   $��0,0,0,10q�65   6G   �� 5      $��Selectează tratamentele:��6   $
��Bold"�68   �� 5      -   5        $��lstTratamente��5      $	��150��6+   $��0,0,0,10q�68    �� SelectionMode$ Extended=�6J   �� 5      $��Metodă de plată:��6   $
��Bold"�60   �� 5      -   5        $��cmbMetodaPlata��5      $��30��6-   $��0,0,0,10q�69   .@�5      �� $��Numerar��6   �� 5      $
��Card��6   �� 5      $��Transfer bancar��6   5      �� 5"      $��Total (lei):��6   $
��Bold"�6,   �� 5#      -   5        $��txtTotal��5#      $��30��6&   $��0,0,0,10q�62    �� 
IsReadOnly . ���#eee6V   5$      �� 5&   
   -   5        $��2��5&      $��Generează factura��6   $��35��6:   $	��160��6F   $��Center=�6R   $��0 20 0 0q�5'      ���#4CAF50
�������$
��Bold"�5(   9   5)      5*      