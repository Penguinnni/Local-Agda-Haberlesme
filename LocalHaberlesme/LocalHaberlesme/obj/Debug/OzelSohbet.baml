   M S B A M L   `   `   `  ����K  GLocal Haberlesme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:LocalHaberlesmeLocalHaberlesme  5           LocalHaberlesme.OzelSohbet   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      )localclr-namespace:LocalHaberlesme  5        N� Title$  
OzelSohbet��5	      $	��450��6   $	��800��6'   .��5      � .+�5   
   � .+�5      �� -   5         ��Name$ turuncuCerceve��5      $��1q�6-    �� CornerRadius$ 40��6A   $
��Left=�6S   $	��450��6n   $	��Top=�6{   $	��800��6�   ���	#FFFF9800._�5      �� $��1q�6   $ 40��6-   $��4,4,4,4q�6?   ���	#FF202225._�5      � .+�5      �� -   5         ��Name$
 kAdi��5       $��Kullanıcı Adı:��6.   $
��Left=�6G   $��42,3,0,0q�6b   $	��Top=�6t   ���	#FFFF9800$
��Bold"�6�   $��20(�6�   $��39��6�   $	��648��6�   �� 5      -   5         ��Name$ Sohbet��5   "    �� TextWrapping$
 Wrap=�62   $�� ��6F   " �� VerticalScrollBarVisibility$
 Auto=�6N   $ �� HorizontalScrollBarVisibility$
 Auto=�6q    �� 
IsReadOnly . $��17(�6�   ���	#FF36393F���	#FF36393F
�������	 �� SelectionBrush	 �	#FFFF9800$��
42,42,42,0q�6  $	��276��6'  $	��Top=�64  .��6m  
 J� Effect
 5      3  -System.Windows.Media.Effects.DropShadowEffect  5   "      
BlurRadius$ 10Q�63      ShadowDepth$ 7Q�6C   .��5   .   5      �� 5      -   5         ��Name$ temizlebuton��5   !    &  "MaterialDesignFloatingActionButton#���   67   ���	#FF36393F$��695,278,62,0q�6�   $��25��6�   $��25��6�   $	��Top=�6�   .��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Delete=�66   $��Center=�6D   $��21��6a   $��21��6m   5      � 5      -   5         �Name$ 	mesajgrid��5      $��
0,350,42,0q�62   $��50��6F   $	��Top=�6R   ���	#FF36393F$��Right=�6�   $	��704��6�   
 5        5   "   $ 10Q�63   $ 7Q�6C   .+�5      �� -   5        $ Mesaj��5   &   $
 Wrap=�65   
�������
���   �$��17(�6�   	 �	#FFFF9800
 � Tag$  ��6�   ��6I   Z� ��6l   Z� 6�   �� 5       -   5        $ gonderbuton��5    %   #���   6:   $��653,13,20,12q�6w   $��25��6�   $��25��6�   ���	#FF36393F.��5!   "     $
 Send=�6:   $��Center=�6F   5#      5%      5(      5)      5*      �� 5+      -   5        $ kapat��5+      #���   6$   $��753,22,22,0q�6a   $	��Top=�6v   $��25��6�   $��25��6�   ���	#FF202225.��5,        $ Close=�6*   $��20��67   $��20��6C   5-      5/      50      51      