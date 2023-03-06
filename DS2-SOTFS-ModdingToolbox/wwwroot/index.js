const body = document.body

const baseSize = {
    width: 960,
    height: 540
}

setInterval(() =>
{
    const width = window.innerWidth //body.offsetWidth
    const height = window.innerHeight //body.offsetHeight
    const zoom = Math.min(height / baseSize.height, width / baseSize.width)
    body.style.zoom = zoom
}, 15)