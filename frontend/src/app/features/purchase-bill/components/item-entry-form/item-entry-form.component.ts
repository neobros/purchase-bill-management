import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PurchaseBillItem } from '../../../../core/models/purchase-bill.model';
import { AutocompleteInputComponent } from '../../../../shared/components/autocomplete-input/autocomplete-input.component';
import {
  SelectDropdownComponent,
  SelectOption,
} from '../../../../shared/components/select-dropdown/select-dropdown.component';

@Component({
  selector: 'app-item-entry-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AutocompleteInputComponent, SelectDropdownComponent],
  templateUrl: './item-entry-form.component.html',
  styleUrl: './item-entry-form.component.scss',
})
export class ItemEntryFormComponent implements OnChanges {
  @Input() itemOptions: string[] = [];
  @Input() batchOptions: SelectOption[] = [];
  @Input() editingItem: PurchaseBillItem | null = null;
  @Output() add = new EventEmitter<PurchaseBillItem>();
  @Output() cancelEdit = new EventEmitter<void>();

  private readonly fb = inject(FormBuilder);

  readonly form = this.fb.group({
    item: ['', Validators.required],
    batch: ['', Validators.required],
    standardCost: [0, [Validators.required, Validators.min(0)]],
    standardPrice: [0, [Validators.required, Validators.min(0)]],
    qty: [0, [Validators.required, Validators.min(1)]],
    freeQty: [0, [Validators.min(0)]],
    discount: [0, [Validators.min(0), Validators.max(100)]],
  });

  get margin(): number {
    const { standardCost, standardPrice } = this.form.value;
    return (standardPrice ?? 0) - (standardCost ?? 0);
  }

  get totalCost(): number {
    const { standardCost, qty, discount } = this.form.value;
    const subtotal = (standardCost ?? 0) * (qty ?? 0);
    return subtotal - (subtotal * (discount ?? 0)) / 100;
  }

  get totalSelling(): number {
    const { standardPrice, qty } = this.form.value;
    return (standardPrice ?? 0) * (qty ?? 0);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes['editingItem']) {
      return;
    }

    if (this.editingItem) {
      this.form.setValue({
        item: this.editingItem.item,
        batch: this.editingItem.batch,
        standardCost: this.editingItem.standardCost,
        standardPrice: this.editingItem.standardPrice,
        qty: this.editingItem.qty,
        freeQty: this.editingItem.freeQty,
        discount: this.editingItem.discount,
      });
    } else {
      this.resetForm();
    }
  }

  onAdd(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { item, batch, standardCost, standardPrice, qty, freeQty, discount } = this.form.value;

    this.add.emit({
      id: this.editingItem?.id ?? Date.now(),
      item: item!,
      batch: batch!,
      standardCost: standardCost ?? 0,
      standardPrice: standardPrice ?? 0,
      margin: this.margin,
      qty: qty ?? 0,
      freeQty: freeQty ?? 0,
      discount: discount ?? 0,
      totalCost: this.totalCost,
      totalSelling: this.totalSelling,
    });

    this.resetForm();
  }

  onCancelEdit(): void {
    this.cancelEdit.emit();
    this.resetForm();
  }

  private resetForm(): void {
    this.form.reset({
      item: '',
      batch: '',
      standardCost: 0,
      standardPrice: 0,
      qty: 0,
      freeQty: 0,
      discount: 0,
    });
  }
}
