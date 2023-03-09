const body = document.body

const baseSize = {
    width: 960,
    height: 540
}

setInterval(() =>
{
    const width = window.innerWidth
    const height = window.innerHeight
    const zoom = Math.min(height / baseSize.height, width / baseSize.width)
    body.style.zoom = zoom
}, 15)