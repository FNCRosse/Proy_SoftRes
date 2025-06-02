<script>
    document.addEventListener("DOMContentLoaded", function () {
        "use strict";

    // Toggle the side navigation
    const toggleButtons = document.querySelectorAll("#sidebarToggle, #sidebarToggleTop");
  toggleButtons.forEach(btn => {
        btn.addEventListener("click", function () {
            document.body.classList.toggle("sidebar-toggled");
            const sidebar = document.querySelector(".sidebar");
            if (sidebar) {
                sidebar.classList.toggle("toggled");

                if (sidebar.classList.contains("toggled")) {
                    const collapses = sidebar.querySelectorAll(".collapse");
                    collapses.forEach(col => {
                        if (col.classList.contains("show")) {
                            new bootstrap.Collapse(col, { toggle: false }).hide();
                        }
                    });
                }
            }
        });
  });

    // Collapse menu on resize
    window.addEventListener("resize", function () {
    const sidebar = document.querySelector(".sidebar");
    if (window.innerWidth < 768) {
      const collapses = sidebar.querySelectorAll(".collapse");
      collapses.forEach(col => {
        if (col.classList.contains("show")) {
        new bootstrap.Collapse(col, { toggle: false }).hide();
        }
      });
    }

    if (window.innerWidth < 480 && !sidebar.classList.contains("toggled")) {
        document.body.classList.add("sidebar-toggled");
    sidebar.classList.add("toggled");
    const collapses = sidebar.querySelectorAll(".collapse");
      collapses.forEach(col => {
        if (col.classList.contains("show")) {
        new bootstrap.Collapse(col, { toggle: false }).hide();
        }
      });
    }
  });

    // Scroll to top button appear
    document.addEventListener("scroll", function () {
    const scrollDistance = window.scrollY;
    const scrollToTopBtn = document.querySelector('.scroll-to-top');
    if (scrollToTopBtn) {
        scrollToTopBtn.style.display = scrollDistance > 100 ? 'block' : 'none';
    }
  });

  // Smooth scrolling to top
  document.querySelectorAll('a.scroll-to-top').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                window.scrollTo({
                    top: target.offsetTop,
                    behavior: 'smooth'
                });
            }
        });
  });
});
</script>
