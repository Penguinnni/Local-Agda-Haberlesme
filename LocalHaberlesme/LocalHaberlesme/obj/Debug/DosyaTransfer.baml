   M S B A M L   `   `   `  ����K  GLocal Haberlesme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:LocalHaberlesmeLocalHaberlesme  5      #    LocalHaberlesme.DosyaTransfer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      )localclr-namespace:LocalHaberlesme  5        N� Title$  DosyaTransfer��5	      $	��450��6   $	��800��6*   .��5      � .+�5   
   � .+�5      �� $��1q�6    �� CornerRadius$ 40��6)   $
��Left=�6;   $	��450��6V   $	��Top=�6c   $	��800��6{   ���	#FFFF9800._�5      �� $��1q�6   $ 40��6-   $��4,4,4,4q�6?   ���	#FF202225._�5      � .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dg��5   #   $��28,50q�6/   
���   �
�������   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5   "   ,  &System.Windows.Controls.DataGridColumn   Header$
 Saat��6T      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6t   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   ��  �� Path$
 Saat�6t     5   "   $ 
Dosya Kodu��6S    . $
 Auto 6y    65   �� $	 Kod�6y     5   "   $ 	Gönderen��6W    . $
 Auto 6{    65   �� $ Gonderen�6{     5   "   $ Dosya��6U    . $
 Auto 6v    65   �� $ Dosya�6v     5   "   $ Boyut��6U    . $
 Auto 6v    65   �� $ Boyut�6v   5      �� 5      -   5        	 ��Name$	 	dosyaIste��5   !   +  %MaterialDesignThemes.Wpf.ButtonAssist
   CornerRadius$
 20��64   $��Seçilen Dosyayı İndir��6b   $��35��6�   $��398,10,200,0q�6�   $	��Top=�6�   6�   �� 5      -   5        $	 	gonderBtn��5   !   $
 20��64   $��Dosya Gönder��6b   $��35��6y   $��200,10,398,0q�6�   $	��Top=�6�   6�   5      5      5      �� 5      -   5        $	 kapat��5       &  "MaterialDesignFloatingActionButton#���   6$   $��753,22,22,0q�6a   $	��Top=�6v   $��25��6�   $��25��6�   ���	#FF202225.��5       '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=�6*   $��20��67   $��20��6C   5!      5$      �� 5%   
   -   5        $	 klasorAc��5%      $
 20��6:   $$��İndirilenler Klasörünü Aç��6h   $��35��6�   $��305,402,305,0q�6�   $	��Top=�6�   5&      5'      