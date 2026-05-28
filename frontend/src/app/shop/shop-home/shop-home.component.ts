import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-shop-home',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './shop-home.component.html',
  styleUrl: './shop-home.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShopHomeComponent {}
