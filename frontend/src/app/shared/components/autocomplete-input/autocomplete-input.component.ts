import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, Input, forwardRef, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-autocomplete-input',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './autocomplete-input.component.html',
  styleUrl: './autocomplete-input.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AutocompleteInputComponent),
      multi: true,
    },
  ],
})
export class AutocompleteInputComponent implements ControlValueAccessor {
  @Input() options: string[] = [];
  @Input() placeholder = '';
  @Input() required = false;

  readonly query = signal('');
  readonly isOpen = signal(false);
  readonly disabled = signal(false);

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  constructor(private readonly elementRef: ElementRef<HTMLElement>) {}

  get filteredOptions(): string[] {
    const term = this.query().toLowerCase().trim();
    if (!term) {
      return this.options;
    }
    return this.options.filter((option) => option.toLowerCase().includes(term));
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target as Node)) {
      this.isOpen.set(false);
    }
  }

  onInput(value: string): void {
    this.query.set(value);
    this.onChange(value);
    this.isOpen.set(true);
  }

  onFocus(): void {
    this.isOpen.set(true);
  }

  onBlur(): void {
    this.onTouched();
  }

  selectOption(option: string): void {
    this.query.set(option);
    this.onChange(option);
    this.isOpen.set(false);
  }

  writeValue(value: string): void {
    this.query.set(value ?? '');
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }
}
