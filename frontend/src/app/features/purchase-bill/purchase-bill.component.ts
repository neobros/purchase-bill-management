import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { LocationService } from '../../core/services/location.service';
import { PurchaseBillService } from '../../core/services/purchase-bill.service';
import { UserLocation } from '../../core/models/auth.model';
import {
  FinancialSummary,
  ItemSummary,
  PurchaseBillItem,
  PurchaseBillItemResponse,
  PurchaseBillResponse,
  TaxSummary,
} from '../../core/models/purchase-bill.model';
import { ItemEntryFormComponent } from './components/item-entry-form/item-entry-form.component';
import { ItemTableComponent } from './components/item-table/item-table.component';
import { SummaryPanelComponent } from './components/summary-panel/summary-panel.component';
import { BillHistoryComponent } from './components/bill-history/bill-history.component';
import { SelectOption } from '../../shared/components/select-dropdown/select-dropdown.component';
import { environment } from '../../../environments/environment';

type PrimaryTab = 'Header' | 'Details' | 'Notes' | 'Documents';
type DetailTab = 'Items' | 'Accounts' | 'Taxes';

const ITEM_OPTIONS = ['Mango', 'Apple', 'Banana', 'Orange', 'Grapes', 'Kiwi', 'Strawberry'];

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [
    CommonModule,
    ItemEntryFormComponent,
    ItemTableComponent,
    SummaryPanelComponent,
    BillHistoryComponent,
  ],
  templateUrl: './purchase-bill.component.html',
  styleUrl: './purchase-bill.component.scss',
})
export class PurchaseBillComponent implements OnInit {
  readonly primaryTabs: PrimaryTab[] = ['Header', 'Details', 'Notes', 'Documents'];
  readonly detailTabs: DetailTab[] = ['Items', 'Accounts', 'Taxes'];

  readonly activePrimaryTab = signal<PrimaryTab>('Details');
  readonly activeDetailTab = signal<DetailTab>('Items');

  readonly itemOptions = ITEM_OPTIONS;

  readonly items = signal<PurchaseBillItem[]>([]);

  readonly saving = signal(false);
  readonly saveError = signal<string | null>(null);
  readonly saveSuccess = signal<string | null>(null);

  readonly fetchedLocations = signal<UserLocation[]>([]);
  readonly loadingLocations = signal(false);

  readonly bills = signal<PurchaseBillResponse[]>([]);
  readonly loadingBills = signal(false);

  readonly editingBillId = signal<number | null>(null);
  readonly deleteError = signal<string | null>(null);

  readonly editingItem = signal<PurchaseBillItem | null>(null);

  readonly batchOptions = computed<SelectOption[]>(() => {
    const locations = this.fetchedLocations().length
      ? this.fetchedLocations()
      : this.authService.locations().length
        ? this.authService.locations()
        : environment.bypassAuth
          ? environment.mockLocations
          : [];

    return locations.map((location) => ({
      value: location.locationCode,
      label: location.locationName,
    }));
  });

  readonly itemSummary = computed<ItemSummary>(() => ({
    totalItems: this.items().length,
    totalQty: this.items().reduce((sum, row) => sum + row.qty, 0),
  }));

  readonly financialSummary = computed<FinancialSummary>(() => {
    const rows = this.items();
    const grossTotal = rows.reduce((sum, row) => sum + row.totalSelling, 0);
    const itemDiscount = rows.reduce(
      (sum, row) => sum + (row.standardCost * row.qty - row.totalCost),
      0,
    );
    const overallDiscount = 0;
    const totalDiscounts = itemDiscount + overallDiscount;
    const totalBeforeTax = grossTotal - totalDiscounts;

    return { grossTotal, itemDiscount, overallDiscount, totalDiscounts, totalBeforeTax };
  });

  readonly taxSummary = computed<TaxSummary>(() => ({ totalTax: 0, suspendedTax: 0 }));

  readonly netTotal = computed(
    () => this.financialSummary().totalBeforeTax + this.taxSummary().totalTax - this.taxSummary().suspendedTax,
  );

  constructor(
    private readonly authService: AuthService,
    private readonly locationService: LocationService,
    private readonly purchaseBillService: PurchaseBillService,
    private readonly router: Router,
  ) {}

  ngOnInit(): void {
    this.refreshLocations();
    this.loadBills();
  }

  loadBills(): void {
    if (environment.bypassAuth) {
      return;
    }

    this.loadingBills.set(true);
    this.purchaseBillService
      .getAll()
      .pipe(finalize(() => this.loadingBills.set(false)))
      .subscribe({
        next: (bills) => this.bills.set(bills),
        error: () => {
          // Leave the previously loaded history in place if the refresh call fails.
        },
      });
  }

  refreshLocations(): void {
    if (environment.bypassAuth) {
      return;
    }

    this.loadingLocations.set(true);
    this.locationService
      .getAll()
      .pipe(finalize(() => this.loadingLocations.set(false)))
      .subscribe({
        next: (locations) => this.fetchedLocations.set(locations),
        error: () => {
          // Fall back to the locations captured at login time if the refresh call fails.
        },
      });
  }

  setPrimaryTab(tab: PrimaryTab): void {
    this.activePrimaryTab.set(tab);
  }

  setDetailTab(tab: DetailTab): void {
    this.activeDetailTab.set(tab);
  }

  onAddItem(item: PurchaseBillItem): void {
    this.items.update((rows) => {
      const index = rows.findIndex((row) => row.id === item.id);
      if (index === -1) {
        return [...rows, item];
      }
      const next = [...rows];
      next[index] = item;
      return next;
    });
    this.editingItem.set(null);
  }

  onRemoveItem(id: number): void {
    this.items.update((rows) => rows.filter((row) => row.id !== id));
    if (this.editingItem()?.id === id) {
      this.editingItem.set(null);
    }
  }

  onEditItemRow(item: PurchaseBillItem): void {
    this.editingItem.set(item);
  }

  onCancelItemEdit(): void {
    this.editingItem.set(null);
  }

  onEditBill(bill: PurchaseBillResponse): void {
    this.editingBillId.set(bill.id);
    this.items.set(bill.items.map((item: PurchaseBillItemResponse) => ({ ...item })));
    this.editingItem.set(null);
    this.saveError.set(null);
    this.saveSuccess.set(null);
    this.deleteError.set(null);
    this.setDetailTab('Items');
  }

  cancelEdit(): void {
    this.editingBillId.set(null);
    this.items.set([]);
    this.editingItem.set(null);
    this.saveError.set(null);
    this.saveSuccess.set(null);
  }

  onDeleteBill(id: number): void {
    if (!confirm(`Delete Purchase Bill #${id}? This cannot be undone.`)) {
      return;
    }

    this.deleteError.set(null);

    this.purchaseBillService.delete(id).subscribe({
      next: () => {
        if (this.editingBillId() === id) {
          this.cancelEdit();
        }
        this.loadBills();
      },
      error: (err) => {
        this.deleteError.set(
          err?.error?.detail ?? err?.error?.message ?? 'Failed to delete the purchase bill.',
        );
      },
    });
  }

  saveBill(): void {
    this.saveError.set(null);
    this.saveSuccess.set(null);

    if (this.items().length === 0) {
      this.saveError.set('Add at least one item before saving.');
      return;
    }

    this.saving.set(true);

    const request = {
      billDate: new Date().toISOString(),
      items: this.items().map((row) => ({
        item: row.item,
        batch: row.batch,
        standardCost: row.standardCost,
        standardPrice: row.standardPrice,
        margin: row.margin,
        qty: row.qty,
        freeQty: row.freeQty,
        discount: row.discount,
      })),
    };

    const editingId = this.editingBillId();
    const save$ = editingId ? this.purchaseBillService.update(editingId, request) : this.purchaseBillService.create(request);

    save$.pipe(finalize(() => this.saving.set(false))).subscribe({
      next: (bill) => {
        this.saveSuccess.set(editingId ? `Purchase bill #${bill.id} updated.` : `Purchase bill #${bill.id} saved.`);
        this.items.set([]);
        this.editingBillId.set(null);
        this.editingItem.set(null);
        this.loadBills();
      },
      error: (err) => {
        this.saveError.set(
          err?.error?.detail ?? err?.error?.message ?? 'Failed to save the purchase bill.',
        );
      },
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
