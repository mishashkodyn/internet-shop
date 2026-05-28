import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-blog-home',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './blog-home.component.html',
  styleUrl: './blog-home.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BlogHomeComponent {}
