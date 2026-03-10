// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Global JavaScript for LTW
// Contains common functions and utilities used across the application
(function() {
    'use strict';

    // Initialize when DOM is ready
    document.addEventListener('DOMContentLoaded', function() {
        initializeComponents();
        setupEventListeners();
        setupAnimations();
    });

    // Initialize all components
    function initializeComponents() {
        initializeTooltips();
        initializePopovers();
        initializeFormValidation();
        initializeLoadingStates();
        initializeScrollEffects();
    }

    // Setup event listeners
    function setupEventListeners() {
        setupFormSubmissions();
        setupNavigationEffects();
        setupModalHandlers();
        setupSearchFunctionality();
    }

    // Initialize Bootstrap tooltips
    function initializeTooltips() {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Initialize Bootstrap popovers
    function initializePopovers() {
        var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
        var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl);
        });
    }

    // Initialize form validation
    function initializeFormValidation() {
        // Custom validation for password strength
        const passwordInputs = document.querySelectorAll('input[type="password"]');
        passwordInputs.forEach(input => {
            input.addEventListener('input', function() {
                validatePasswordStrength(this);
            });
        });

        // Custom validation for phone numbers
        const phoneInputs = document.querySelectorAll('input[type="tel"]');
        phoneInputs.forEach(input => {
            input.addEventListener('input', function() {
                formatPhoneNumber(this);
            });
        });

        // Auto-save form data
        const forms = document.querySelectorAll('form[data-auto-save]');
        forms.forEach(form => {
            setupAutoSave(form);
        });
    }

    // Validate password strength
    function validatePasswordStrength(input) {
        const password = input.value;
        const strengthIndicator = input.parentElement.querySelector('.password-strength');
        
        if (!strengthIndicator) return;

        let strength = 0;
        let feedback = '';

        if (password.length >= 6) strength++;
        if (password.match(/[a-z]/)) strength++;
        if (password.match(/[A-Z]/)) strength++;
        if (password.match(/[0-9]/)) strength++;
        if (password.match(/[^a-zA-Z0-9]/)) strength++;

        switch (strength) {
            case 0:
            case 1:
                feedback = 'Rất yếu';
                strengthIndicator.className = 'password-strength text-danger';
                break;
            case 2:
                feedback = 'Yếu';
                strengthIndicator.className = 'password-strength text-warning';
                break;
            case 3:
                feedback = 'Trung bình';
                strengthIndicator.className = 'password-strength text-info';
                break;
            case 4:
                feedback = 'Mạnh';
                strengthIndicator.className = 'password-strength text-success';
                break;
            case 5:
                feedback = 'Rất mạnh';
                strengthIndicator.className = 'password-strength text-success';
                break;
        }

        strengthIndicator.textContent = feedback;
    }

    // Format phone number
    function formatPhoneNumber(input) {
        let value = input.value.replace(/\D/g, '');
        
        if (value.length > 0) {
            if (value.length <= 3) {
                value = value;
            } else if (value.length <= 6) {
                value = value.slice(0, 3) + ' ' + value.slice(3);
            } else {
                value = value.slice(0, 3) + ' ' + value.slice(3, 6) + ' ' + value.slice(6, 10);
            }
        }
        
        input.value = value;
    }

    // Setup auto-save for forms
    function setupAutoSave(form) {
        const formData = new FormData(form);
        const formId = form.id || 'form_' + Math.random().toString(36).substr(2, 9);
        
        // Load saved data
        const savedData = localStorage.getItem('form_' + formId);
        if (savedData) {
            try {
                const data = JSON.parse(savedData);
                Object.keys(data).forEach(key => {
                    const input = form.querySelector(`[name="${key}"]`);
                    if (input && !input.value) {
                        input.value = data[key];
                    }
                });
            } catch (e) {
                console.error('Error loading saved form data:', e);
            }
        }

        // Save data on input change
        form.addEventListener('input', function(e) {
            const formData = new FormData(form);
            const data = {};
            
            for (let [key, value] of formData.entries()) {
                data[key] = value;
            }
            
            localStorage.setItem('form_' + formId, JSON.stringify(data));
        });
    }

    // Initialize loading states
    function initializeLoadingStates() {
        // Add loading state to buttons on form submission
        const forms = document.querySelectorAll('form');
        forms.forEach(form => {
            form.addEventListener('submit', function() {
                const submitBtn = form.querySelector('button[type="submit"]');
                if (submitBtn) {
                    submitBtn.disabled = true;
                    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Đang xử lý...';
                }
            });
        });
    }

    // Initialize scroll effects
    function initializeScrollEffects() {
        // Smooth scroll for anchor links
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function (e) {
                e.preventDefault();
                const target = document.querySelector(this.getAttribute('href'));
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            });
        });

        // Parallax effect for background images
        window.addEventListener('scroll', function() {
            const scrolled = window.pageYOffset;
            const parallaxElements = document.querySelectorAll('.parallax');
            
            parallaxElements.forEach(element => {
                const speed = element.dataset.speed || 0.5;
                element.style.transform = `translateY(${scrolled * speed}px)`;
            });
        });
    }

    // Setup form submissions
    function setupFormSubmissions() {
        // Add confirmation for delete actions
        const deleteButtons = document.querySelectorAll('[data-confirm]');
        deleteButtons.forEach(button => {
            button.addEventListener('click', function(e) {
                const message = this.dataset.confirm || 'Bạn có chắc chắn muốn thực hiện hành động này?';
                if (!confirm(message)) {
                    e.preventDefault();
                }
            });
        });

        // Add file upload preview
        const fileInputs = document.querySelectorAll('input[type="file"]');
        fileInputs.forEach(input => {
            input.addEventListener('change', function() {
                previewFile(this);
            });
        });
    }

    // Preview uploaded file
    function previewFile(input) {
        const preview = input.parentElement.querySelector('.file-preview');
        if (!preview) return;

        const file = input.files[0];
        if (file) {
            if (file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    preview.innerHTML = `<img src="${e.target.result}" class="img-thumbnail" style="max-height: 150px;">`;
                };
                reader.readAsDataURL(file);
            } else {
                preview.innerHTML = `<div class="alert alert-info">File: ${file.name}</div>`;
            }
        } else {
            preview.innerHTML = '';
        }
    }

    // Setup navigation effects
    function setupNavigationEffects() {
        // Add active class to current nav item
        const currentPath = window.location.pathname;
        const navLinks = document.querySelectorAll('.nav-link');
        
        navLinks.forEach(link => {
            if (link.getAttribute('href') === currentPath) {
                link.classList.add('active');
            }
        });

        // Smooth dropdown animations
        const dropdowns = document.querySelectorAll('.dropdown');
        dropdowns.forEach(dropdown => {
            const menu = dropdown.querySelector('.dropdown-menu');
            if (menu) {
                dropdown.addEventListener('mouseenter', function() {
                    menu.style.display = 'block';
                    setTimeout(() => menu.classList.add('show'), 10);
                });
                
                dropdown.addEventListener('mouseleave', function() {
                    menu.classList.remove('show');
                    setTimeout(() => menu.style.display = 'none', 300);
                });
            }
        });
    }

    // Setup modal handlers
    function setupModalHandlers() {
        // Auto-focus first input in modals
        const modals = document.querySelectorAll('.modal');
        modals.forEach(modal => {
            modal.addEventListener('shown.bs.modal', function() {
                const firstInput = this.querySelector('input, textarea, select');
                if (firstInput) {
                    firstInput.focus();
                }
            });
        });
    }

    // Setup search functionality
    function setupSearchFunctionality() {
        const searchInputs = document.querySelectorAll('.search-input');
        searchInputs.forEach(input => {
            let searchTimeout;
            
            input.addEventListener('input', function() {
                clearTimeout(searchTimeout);
                searchTimeout = setTimeout(() => {
                    performSearch(this.value);
                }, 300);
            });
        });
    }

    // Perform search
    function performSearch(query) {
        // Implement search functionality here
        console.log('Searching for:', query);
    }

    // Setup animations
    function setupAnimations() {
        // Intersection Observer for fade-in animations
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver(function(entries) {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('fade-in');
                }
            });
        }, observerOptions);

        // Observe elements with animation classes
        document.querySelectorAll('.animate-on-scroll').forEach(el => {
            observer.observe(el);
        });

        // Add hover effects
        document.querySelectorAll('.hover-effect').forEach(el => {
            el.addEventListener('mouseenter', function() {
                this.style.transform = 'scale(1.05)';
            });
            
            el.addEventListener('mouseleave', function() {
                this.style.transform = 'scale(1)';
            });
        });
    }

    // Utility functions
    window.LTW = {
        // Show notification
        showNotification: function(message, type = 'info') {
            const notification = document.createElement('div');
            notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
            notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
            notification.innerHTML = `
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            `;
            
            document.body.appendChild(notification);
            
            // Auto remove after 5 seconds
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.remove();
                }
            }, 5000);
        },

        // Format currency
        formatCurrency: function(amount, currency = 'VND') {
            return new Intl.NumberFormat('vi-VN', {
                style: 'currency',
                currency: currency
            }).format(amount);
        },

        // Format date
        formatDate: function(date, format = 'dd/MM/yyyy') {
            const d = new Date(date);
            const day = String(d.getDate()).padStart(2, '0');
            const month = String(d.getMonth() + 1).padStart(2, '0');
            const year = d.getFullYear();
            
            return format
                .replace('dd', day)
                .replace('MM', month)
                .replace('yyyy', year);
        },

        // Debounce function
        debounce: function(func, wait) {
            let timeout;
            return function executedFunction(...args) {
                const later = () => {
                    clearTimeout(timeout);
                    func(...args);
                };
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
            };
        },

        // Throttle function
        throttle: function(func, limit) {
            let inThrottle;
            return function() {
                const args = arguments;
                const context = this;
                if (!inThrottle) {
                    func.apply(context, args);
                    inThrottle = true;
                    setTimeout(() => inThrottle = false, limit);
                }
            };
        }
    };

})();
