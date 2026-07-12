import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { PurchaseBillResponse } from '../../../../core/models/purchase-bill.model';

@Component({
  selector: 'app-bill-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bill-history.component.html',
  styleUrl: './bill-history.component.scss',
})
export class BillHistoryComponent {
  @Input() bills: PurchaseBillResponse[] = [];
  @Input() loading = false;

  @Output() edit = new EventEmitter<PurchaseBillResponse>();
  @Output() delete = new EventEmitter<number>();

  readonly expandedId = signal<number | null>(null);

  toggleExpand(id: number): void {
    this.expandedId.update((current) => (current === id ? null : id));
  }
}
